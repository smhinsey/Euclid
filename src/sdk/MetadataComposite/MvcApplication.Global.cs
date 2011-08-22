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

			this._composite = new MvcCompositeApp(container);

			var compositeAppSettings = new CompositeAppSettings();

			/*
           * jt: this is how a composite developer would override the default settings for the mvc composite
           * euclidCompositeConfiguration.BlobStorage.ApplyOverride(typeof(SomeBlobStorageImplementation));
           */
			this._composite.Configure(compositeAppSettings);

			/*
          jt: this is going to be injected into composites adding this pacage
          */
			this._composite.AddAgent(typeof(PublishPost).Assembly);

			// composite.RegisterInputModel(new InputToFakeCommand4Converter());
			container.Register(Component.For<ICompositeApp>().Instance(this._composite));

			this.Error += this._composite.LogUnhandledException;

			this.BeginRequest += this._composite.BeginPageRequest;

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