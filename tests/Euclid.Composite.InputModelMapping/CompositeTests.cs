using Castle.MicroKernel.Registration;
using Castle.Windsor;
using CommonServiceLocator.WindsorAdapter;
using Euclid.Common.Messaging;
using Euclid.Common.Storage;
using Euclid.Common.Storage.Binary;
using Euclid.Common.Storage.Record;
using Euclid.Composites;
using Euclid.Framework.Cqrs;
using Microsoft.Practices.ServiceLocation;
using NUnit.Framework;

namespace Euclid.Composite.InputModelMapping
{
    [TestFixture]
    public class CompositeTests
    {
         [Test]
        public void TestDispatcherResolution()
         {
             var s = new CompositeAppSettings();
             var c = new WindsorContainer();

             c.Register(
                 Component.For<ICommandDispatcher>().ImplementedBy<CommandDispatcher>());

             c.Register(
                 Component.For<ICommandRegistry>().ImplementedBy<CommandRegistry>());

             c.Register(
                 Component.For<IServiceLocator>().ImplementedBy<WindsorServiceLocator>());

             c.Register(
                 Component.For<IWindsorContainer>().Instance(c));

             c.Register(
                 Component.For<IRecordMapper<CommandPublicationRecord>>().ImplementedBy
                     <InMemoryCommandPublicationRecordMapper>());

             c.Register(
                 Component.For<IBlobStorage>().ImplementedBy<InMemoryBlobStorage>());

             c.Register(
                 Component.For<IMessageSerializer>().ImplementedBy<JsonMessageSerializer>());

             var d = c.Resolve<ICommandDispatcher>();
             Assert.NotNull(d);

         }
    }
}