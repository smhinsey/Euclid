using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace Euclid.Composites.Mvc.ComponentRegistration
{
	public class ContainerInstaller : ComponentRegistrationBase
	{
		public override void Install(IWindsorContainer container, IConfigurationStore store)
		{
			container.Kernel.Resolver.AddSubResolver(new ArrayResolver(container.Kernel));

			container.Kernel.Resolver.AddSubResolver(new ListResolver(container.Kernel));
		}
	}
}