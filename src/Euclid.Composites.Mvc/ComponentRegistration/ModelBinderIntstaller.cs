using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Euclid.Composites.Agent;
using Euclid.Composites.Mvc.Binders;

namespace Euclid.Composites.Mvc.ComponentRegistration
{
    public class ModelBinderIntstaller : ComponentRegistrationBase
    {
        public override void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Kernel.Resolver.AddSubResolver(new ArrayResolver(container.Kernel));

            container.Kernel.Resolver.AddSubResolver(new ListResolver(container.Kernel));

            foreach (var t in GetTypesThatImplement<IAgentResolutionStrategy>())
            {
                container.Register(
                    Component
                        .For<IAgentResolutionStrategy>()
                        .ImplementedBy(t)
                        .LifeStyle.Transient);
            }

            foreach (var t in GetTypesThatImplement<IEuclidModelBinder>())
            {
                container.Register(
                    Component
                        .For<IEuclidModelBinder>()
                        .ImplementedBy(t)
                        .LifeStyle.Transient);
            }
        }
    }
}