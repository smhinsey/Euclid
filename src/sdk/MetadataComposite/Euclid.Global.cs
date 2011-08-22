using System.Web.Mvc;
using System.Web.Routing;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Euclid.Composites;
using Euclid.Composites.Mvc;
using ForumAgent.Commands;

namespace MetadataComposite
{
	public partial class MvcApplication
	{
		private MvcCompositeApp _composite;

		public override void Init()
		{
			var container = new WindsorContainer();

			_composite = new MvcCompositeApp(container);

			var euclidCompositeConfiguration = new CompositeAppSettings();

			/*
           * jt: this is how a composite developer would override the default settings for the mvc composite
           * euclidCompositeConfiguration.BlobStorage.ApplyOverride(typeof(SomeBlobStorageImplementation));
           */

			_composite.Configure(euclidCompositeConfiguration);

			/*
          jt: this is going to be injected into composites adding this pacage
          */
			_composite.AddAgent(typeof(PublishPost).Assembly);

			// composite.RegisterInputModel(new InputToFakeCommand4Converter());


			container.Register(Component.For<ICompositeApp>().Instance(_composite));


			Error += _composite.LogUnhandledException;

			BeginRequest += _composite.BeginPageRequest;

			base.Init();
		}

		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();

			RegisterGlobalFilters(GlobalFilters.Filters);

			RegisterRoutes(RouteTable.Routes);

		}
	}
}