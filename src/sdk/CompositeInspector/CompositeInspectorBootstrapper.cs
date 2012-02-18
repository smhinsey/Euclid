using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using CompositeInspector.Extensions;
using Euclid.Common.Logging;
using Euclid.Composites;
using Euclid.Framework.Cqrs;
using Euclid.Framework.Models;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Bootstrappers.Windsor;
using Nancy.Conventions;
using Nancy.Responses;
using Nancy.Routing;
using Nancy.Session;
using Nancy.ViewEngines;
using log4net.Config;
using DefaultViewLocationCache = Nancy.ViewEngines.DefaultViewLocationCache;

namespace CompositeInspector
{
	public class CompositeInspectorBootstrapper : WindsorNancyAspNetBootstrapper
	{
		private byte[] _icon;

		private readonly NancyInternalConfiguration _internalConfiguration;

		public CompositeInspectorBootstrapper()
		{
			// NOTE: there is a bug in the Windsor Bootstrapper that prevents the types below from being registered as DiagnosticProviders - will be fixed with 0.1 release
			_internalConfiguration = NancyInternalConfiguration.Default;
			_internalConfiguration.InteractiveDiagnosticProviders.Remove(typeof(DefaultRouteResolver));
			_internalConfiguration.InteractiveDiagnosticProviders.Remove(typeof(DefaultViewLocationCache));
			_internalConfiguration.InteractiveDiagnosticProviders.Remove(typeof(DefaultRouteCacheProvider));
			_internalConfiguration.ViewLocationProvider = typeof(ResourceViewLocationProvider);

		}

		protected override Nancy.Bootstrapper.NancyInternalConfiguration InternalConfiguration
		{
			get { return _internalConfiguration; }
		}

		protected override byte[] DefaultFavIcon
		{
			get
			{
				if (_icon == null)
				{
					using (var resourceStream = GetType().Assembly.GetManifestResourceStream("CompositeInspector2.Assets.NewCo.ico"))
					{
						if (resourceStream != null && resourceStream.Length > 0)
						{
							var tempFavicon = new byte[resourceStream.Length];
							resourceStream.Read(tempFavicon, 0, (int)resourceStream.Length);
							_icon = tempFavicon;
						}
						else
						{
							_icon = base.DefaultFavIcon;
						}
					}
				}

				return _icon;
			}
		}

		protected override void ApplicationStartup(IWindsorContainer container, IPipelines pipelines)
		{
			CookieBasedSessions.Enable(pipelines);

			// upload posted files to blob storage (configured via the composite)
			configurePipelines(pipelines, container);

			base.ApplicationStartup(container, pipelines);
		}

		protected override void ConfigureApplicationContainer(IWindsorContainer existingContainer)
		{
			var app = existingContainer.Resolve<ICompositeApp>();
			foreach (var agent in app.Agents)
			{
				foreach (var query in agent.Queries)
				{
					existingContainer.Register(Component.For<IQuery>().ImplementedBy(query.Type).Named(query.Name));
				}
			}

			foreach (var inputModel in app.InputModels)
			{
				existingContainer.Register(Component.For<IInputModel>().ImplementedBy(inputModel.Type).Named(inputModel.Name));
			}

			existingContainer.Register(Component.For<FileUploader>().ImplementedBy<FileUploader>().LifeStyle.PerWebRequest);

			//This should be the assembly your views are embedded in
			var assembly = GetType().Assembly;

			ResourceViewLocationProvider.RootNamespaces.Add(assembly, "CompositeInspector.Views");

			base.ConfigureApplicationContainer(existingContainer);
		}

		protected override IWindsorContainer GetApplicationContainer()
		{
			if (ApplicationContainer == null)
			{
				var container = DependencyResolver.Current.GetService<IWindsorContainer>();
				container.Kernel.Resolver.AddSubResolver(new CollectionResolver(container.Kernel, true));
				return container;
			}

			return ApplicationContainer;
		}

		protected override void ConfigureConventions(Nancy.Conventions.NancyConventions nancyConventions)
		{
			base.ConfigureConventions(nancyConventions);
			var assembly = GetType().Assembly;
			const string assetRootNamespace = "CompositeInspector.Assets";

			Conventions.StaticContentsConventions.Add(
				EmbeddedStaticContentConventionBuilder.MapVirtualDirectory("composite/_debug_",
																		   string.Concat(assetRootNamespace, "._debug_"),
																		   assembly));

			Conventions.StaticContentsConventions.Add(
				EmbeddedStaticContentConventionBuilder.MapVirtualDirectory("composite/js",
																		   string.Concat(assetRootNamespace, ".Scripts"),
																		   assembly));

			Conventions.StaticContentsConventions.Add(
				EmbeddedStaticContentConventionBuilder.MapVirtualDirectory("composite/css",
																		   string.Concat(assetRootNamespace, ".Styles"),
																		   assembly));

			Conventions.StaticContentsConventions.Add(
				EmbeddedStaticContentConventionBuilder.MapVirtualDirectory("composite/image",
																		   string.Concat(assetRootNamespace, ".Images"),
																		   assembly));

			Conventions.StaticContentsConventions.Add(getTemplateConvention());
		}

		private static void configurePipelines(IPipelines pipelines, IWindsorContainer container)
		{
			pipelines.BeforeRequest.AddItemToEndOfPipeline(
				ctx =>
				{
					var uploader = container.Resolve<FileUploader>();
					uploader.UploadFiles(ctx);
					return null;
				});

			// return errors in the same format requested
			pipelines.OnError.AddItemToEndOfPipeline((ctx, e) =>
			{
				var format = ctx.GetResponseFormat();
				var formatter = container.Resolve<IResponseFormatterFactory>().Create(ctx);

				return e.CreateResponse(format, formatter);
			});
		}

		private Func<NancyContext, string, Response> getTemplateConvention()
		{
			return (context, s) =>
				{
					const string templateRoot = "/composite/ui/template/";
					const string resourceRoot = "CompositeInspector.Views.templates";
					var assembly = GetType().Assembly;
					var resourceName = string.Empty;
					if (context.Request.Path.StartsWith(templateRoot))
					{
						var templateName = context.Request.Path.Substring(templateRoot.Length, context.Request.Path.Length - templateRoot.Length);

						resourceName = String.Concat(resourceRoot, ".", templateName.Replace("/", "."), ".html");
					}

					return
						!string.IsNullOrEmpty(resourceName) && assembly.GetManifestResourceNames().Any(n => n.Equals(resourceName, StringComparison.InvariantCulture))
							? new StreamResponse(() => assembly.GetManifestResourceStream(resourceName), "text/html")
							: null;
				};
		}
	}
}