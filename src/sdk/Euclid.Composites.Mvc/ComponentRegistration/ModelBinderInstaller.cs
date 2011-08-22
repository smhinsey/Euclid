using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Euclid.Composites.AgentResolution;
using Euclid.Composites.Mvc.Binders;

namespace Euclid.Composites.Mvc.ComponentRegistration
{
	public class ModelBinderInstaller : ComponentRegistrationBase
	{
		public override void Install(IWindsorContainer container, IConfigurationStore store)
		{
			foreach (var t in this.GetTypesThatImplement<IAgentResolver>())
			{
				container.Register(Component.For<IAgentResolver>().ImplementedBy(t).LifeStyle.Transient);
			}

			foreach (var t in this.GetTypesThatImplement<IEuclidModelBinder>())
			{
				container.Register(Component.For<IEuclidModelBinder>().ImplementedBy(t).LifeStyle.Transient);
			}
		}
	}
}