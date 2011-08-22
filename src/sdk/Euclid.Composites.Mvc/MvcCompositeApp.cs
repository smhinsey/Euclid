using System;
using System.Web;
using System.Web.Mvc;
using Castle.Windsor;
using Euclid.Common.Logging;
using Euclid.Composites.Mvc.Binders;
using Euclid.Composites.Mvc.ComponentRegistration;

namespace Euclid.Composites.Mvc
{
	public class MvcCompositeApp : BasicCompositeApp
	{
		public MvcCompositeApp(IWindsorContainer container)
			: base(container)
		{
		}

		public void BeginPageRequest(object sender, EventArgs eventArgs)
		{
			if (this.State != CompositeApplicationState.Configured)
			{
				throw new InvalidCompositeApplicationStateException(this.State, CompositeApplicationState.Configured);
			}
		}

		public override void Configure(CompositeAppSettings compositeAppSettings)
		{
			base.Configure(compositeAppSettings);

			this.wireMvcInfrastructure();
		}

		public void LogUnhandledException(object sender, EventArgs eventArgs)
		{
			var e = HttpContext.Current.Server.GetLastError();
			this.WriteFatalMessage(e.Message, e);
		}

		private void wireMvcInfrastructure()
		{
			this.Container.Install(new ModelBinderInstaller());
			this.Container.Install(new ControllerContainerInstaller());

			ModelBinders.Binders.DefaultBinder = new EuclidDefaultBinder(this.Container.ResolveAll<IEuclidModelBinder>());

			ControllerBuilder.Current.SetControllerFactory(new WindsorControllerFactory(this.Container));
		}
	}
}