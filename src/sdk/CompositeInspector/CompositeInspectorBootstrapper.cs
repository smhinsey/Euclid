using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Xml.Serialization;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using Euclid.Common.Storage;
using Euclid.Common.Storage.Binary;
using Euclid.Composites;
using Euclid.Framework.AgentMetadata;
using Euclid.Framework.Cqrs;
using Euclid.Framework.Models;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Bootstrappers.Windsor;
using Nancy.Extensions;
using Nancy.Responses;
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

			pipelines.BeforeRequest.AddItemToEndOfPipeline(
				ctx =>
					{
						var uploader = container.Resolve<FileUploader>();
						uploader.UploadFiles(ctx);
						return null;
					});

			pipelines.OnError.AddItemToEndOfPipeline((ctx, e) =>
			                                         	{
			                                         		var format = ctx.GetResponseFormat();
															var formatter = container.Resolve<IResponseFormatterFactory>().Create(ctx);

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
	}

	public enum ResponseFormat
	{
		Json,
		Xml,
		Html
	}

	public static class FormatExtensions
	{
		public static ResponseFormat GetResponseFormat(this NancyModule module)
		{
			return module.Context.GetResponseFormat();
		}

		public static ResponseFormat GetResponseFormat(this NancyContext ctx)
		{
			var format = ResponseFormat.Html;
			if (
					ctx.Request.Headers.Accept.Any(a => a.IndexOf("application/json", StringComparison.CurrentCultureIgnoreCase) >= 0)
					||
					ctx.Request.Path.EndsWith(".json", StringComparison.CurrentCultureIgnoreCase)
				)
			{
				format = ResponseFormat.Json;
			}
			else if (
				ctx.Request.Headers.Accept.Any(a => a.IndexOf("application/xml", StringComparison.CurrentCultureIgnoreCase) >= 0)
				||
				ctx.Request.Path.EndsWith(".xml", StringComparison.CurrentCultureIgnoreCase)
			)
			{
				format = ResponseFormat.Xml;
			}

			return format;
		}

		public static Response GetFormattedMetadata(this NancyModule module, IMetadataFormatter formatter)
		{
			var format = module.GetResponseFormat();

			var representation = format == ResponseFormat.Json ? "json" : "xml";
			var encodedString = formatter.GetRepresentation(representation);
			var stream = new MemoryStream(Encoding.UTF8.GetBytes(encodedString));
			return module.Response.FromStream(stream, Euclid.Common.Extensions.MimeTypes.GetByExtension(representation));
		}
	}

	public class FileUploader
	{
		private readonly IBlobStorage _blobStorage;

		public FileUploader(IBlobStorage blobStorage)
		{
			_blobStorage = blobStorage;
		}

		public void UploadFiles(NancyContext context)
		{
			foreach (var file in context.Request.Files)
			{
				var key = file.Key + "Url";
				
				Uri blobUrl;
				using (var ms = new MemoryStream())
				{
					file.Value.CopyTo(ms);
					var blob = new Blob
								{
									Content = ms.ToArray(), 
									ContentType = file.ContentType
								};
					blobUrl = _blobStorage.Put(blob, file.Name);
				}

				context.Request.Form[key] = blobUrl.AbsoluteUri;
			}
		}
	}

	[XmlRoot("Exception")]
	public class FormattedException
	{
		public FormattedException()
		{
		}
		public string name { get; set; }
		public string message { get; set; }
		public string callStack { get; set; }
	}
}