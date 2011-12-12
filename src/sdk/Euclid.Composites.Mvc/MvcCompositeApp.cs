using System;
using System.Web;
using System.Web.Mvc;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using CommonServiceLocator.WindsorAdapter;
using Euclid.Common.Logging;
using Euclid.Composites.Mvc.ActionFilters;
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
			if (State != CompositeApplicationState.Configured)
			{
				throw new InvalidCompositeApplicationStateException(State, CompositeApplicationState.Configured);
			}
		}

		public override void Configure(CompositeAppSettings compositeAppSettings)
		{
			base.Configure(compositeAppSettings);

			wireMvcInfrastructure();
		}

		public void LogUnhandledException(object sender, EventArgs eventArgs)
		{
			var e = HttpContext.Current.Server.GetLastError();

			this.WriteFatalMessage(e.Message, e);
		}

		private void wireMvcInfrastructure()
		{
			Container.Install(new ModelBinderInstaller());

			Container.Install(new ControllerContainerInstaller());

			Container.Register(Component.For<IActionInvoker>().ImplementedBy<CompositeActionInvoker>());

			ModelBinders.Binders.DefaultBinder = new EuclidDefaultBinder(Container.ResolveAll<IEuclidModelBinder>());

			ControllerBuilder.Current.SetControllerFactory(new WindsorControllerFactory(Container));

			DependencyResolver.SetResolver(new WindsorDependencyResolver(Container));

			GlobalFilters.Filters.Add(new FormatExceptionAttribute());
		}
	}
}