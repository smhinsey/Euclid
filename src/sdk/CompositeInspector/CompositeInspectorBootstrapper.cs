using System.Web.Mvc;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using Euclid.Composites;
using Euclid.Framework.Cqrs;
using Euclid.Framework.Models;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Bootstrappers.Windsor;
using Nancy.Session;
using Nancy.ViewEngines;

namespace CompositeInspector
{
	public class CompositeInspectorBootstrapper : WindsorNancyAspNetBootstrapper
	{
		private byte[] _icon;

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

		protected override NancyInternalConfiguration InternalConfiguration
		{
			get
			{
				return NancyInternalConfiguration.WithOverrides(x => x.ViewLocationProvider = typeof(ResourceViewLocationProvider));
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
		
		private static void configurePipelines(IPipelines pipelines, IWindsorContainer container)
		{
			pipelines.BeforeRequest.AddItemToEndOfPipeline(
				ctx =>
				{
					var uploader = container.Resolve<FileUploader>();
					uploader.UploadFiles(ctx);
					return null;
				});

			// only allow requests for json/xml through to the apis
			pipelines.BeforeRequest.AddItemToEndOfPipeline(
				ctx => (ctx.Request.Path.StartsWith("composite/api") && ctx.GetResponseFormat() == ResponseFormat.Html)
						? HttpStatusCode.NoContent
						: (Response)null);

			// return errors in the same format requested
			pipelines.OnError.AddItemToEndOfPipeline((ctx, e) =>
			{
				var format = ctx.GetResponseFormat();
				var formatter = container.Resolve<IResponseFormatterFactory>().Create(ctx);

				// dumb ugliness b/c MSFT's xml serializer can't handle anonymous objects
				var exception = new FormattedException
				{
					name = e.GetType().Name,
					message = e.Message,
					callStack = e.StackTrace
				};

				Response r;
				switch (format)
				{
					case ResponseFormat.Json:
						r = formatter.AsJson(exception, HttpStatusCode.InternalServerError);
						break;
					case ResponseFormat.Xml:
						r = formatter.AsXml(exception);
						break;
					default:
						r = null;
						break;
				}

				if (r != null)
				{
					r.StatusCode = HttpStatusCode.InternalServerError;
				}

				return r;
			});
		}
	}
}