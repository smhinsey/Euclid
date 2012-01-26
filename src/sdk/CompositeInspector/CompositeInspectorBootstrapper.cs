using System;
using System.IO;
using System.Web.Mvc;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using Euclid.Common.Storage;
using Euclid.Common.Storage.Binary;
using Euclid.Composites;
using Euclid.Framework.Cqrs;
using Euclid.Framework.Models;
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

			//pipelines.BeforeRequest.AddItemToEndOfPipeline(ctx =>
			//                                                {
			//                                                    var compositeApp = container.Resolve<ICompositeApp>();
			//                                                    ctx.Request.Session["Title"] = string.Format("Inspecting Composite: {0}", compositeApp.Name);
			//                                                    return null;
			//                                                });

			pipelines.BeforeRequest.AddItemToEndOfPipeline(
				ctx =>
					{
						var blobStorage = container.Resolve<IBlobStorage>();

						foreach (var file in ctx.Request.Files)
						{
							var key = file.Key + "Url";
							var blob = container.Resolve<IBlob>();
							Uri blobUrl;
							using (var ms = new MemoryStream())
							{
								file.Value.CopyTo(ms);
								blob.Content = ms.ToArray();
								blob.ContentType = file.ContentType;
								blobUrl = blobStorage.Put(blob, file.Name);
							}

							ctx.Request.Form[key] = blobUrl.AbsoluteUri;
						}

						return null;
					});

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
	}
}