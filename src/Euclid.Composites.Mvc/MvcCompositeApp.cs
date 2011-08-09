using System;
using System.Web;
using System.Web.Mvc;
using Euclid.Common.Logging;
using Euclid.Composites.Mvc.Binders;
using Euclid.Composites.Mvc.ComponentRegistration;

namespace Euclid.Composites.Mvc
{
	public class MvcCompositeApp : DefaultCompositeApp
	{
		public override void Configure(CompositeAppSettings compositeAppSettings)
		{
			base.Configure(compositeAppSettings);

			wireMvcInfrastructure();
		}

		private void wireMvcInfrastructure()
		{
			Container.Install(new ModelBinderInstaller());
			Container.Install(new ControllerContainerInstaller());

			ModelBinders.Binders.DefaultBinder = new EuclidDefaultBinder(Container.ResolveAll<IEuclidModelBinder>());

			ControllerBuilder.Current.SetControllerFactory(new WindsorControllerFactory(Container));
		}

		public void BeginPageRequest(object sender, EventArgs eventArgs)
		{
			if (ApplicationState != CompositeApplicationState.Configured)
			{
				throw new InvalidCompositeApplicationStateException(ApplicationState, CompositeApplicationState.Configured);
			}
		}

		public void LogUnhandledException(object sender, EventArgs eventArgs)
		{
			var e = HttpContext.Current.Server.GetLastError();
			this.WriteFatalMessage(e.Message, e);
		}
	}
}