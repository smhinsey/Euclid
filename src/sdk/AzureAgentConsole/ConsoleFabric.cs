using Castle.MicroKernel.Registration;
using Castle.Windsor;
using CommonServiceLocator.WindsorAdapter;
using Euclid.Common.ServiceHost;
using Euclid.Framework.HostingFabric;
using Microsoft.Practices.ServiceLocation;

namespace AzureAgentConsole
{
	public class ConsoleFabric : BasicFabric
	{
		public ConsoleFabric(IWindsorContainer container)
			: base(container)
		{
			container.Register(Component.For<IServiceLocator>().Instance(new WindsorServiceLocator(Container)));
		}

		public override void Initialize(IFabricRuntimeSettings settings)
		{
			Container.Register(
				Component.For<IServiceHost>().Forward<MultitaskingServiceHost>().Instance(new MultitaskingServiceHost()));

			base.Initialize(settings);
		}
	}
}