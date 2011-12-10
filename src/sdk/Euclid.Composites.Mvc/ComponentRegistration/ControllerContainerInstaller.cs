using System.Web.Mvc;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace Euclid.Composites.Mvc.ComponentRegistration
{
	public class ControllerContainerInstaller : ComponentRegistrationBase
	{
		public override void Install(IWindsorContainer container, IConfigurationStore store)
		{
			foreach (var t in GetTypesThatImplement<IController>())
			{
				container.Register(Component.For(t).ImplementedBy(t).LifeStyle.Transient);
			}
		}
	}
}