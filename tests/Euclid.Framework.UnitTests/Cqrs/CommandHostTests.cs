using System;
using System.Threading;
using Castle.MicroKernel.Registration;
using CommonServiceLocator.WindsorAdapter;
using Euclid.Common.Logging;
using Euclid.Common.Messaging;
using Euclid.Common.ServiceHost;
using Euclid.Common.Storage;
using Castle.Windsor;
using Euclid.Common.Storage.Blob;
using Euclid.Common.Storage.Record;
using Euclid.Framework.Cqrs;
using Euclid.Framework.TestingFakes.Cqrs;
using NUnit.Framework;

namespace Euclid.Framework.UnitTests.Cqrs
{
    [TestFixture]
    public class CommandHostTests
    {
        private IMessageDispatcherSettings _dispatcherSettings;
        private IWindsorContainer _container;
        private WindsorServiceLocator _locator;


        [SetUp]
        public void Setup()
        {
            ConfigureContainer();

            _dispatcherSettings = new MessageDispatcherSettings();

            _dispatcherSettings.InvalidChannel.WithDefault(_container.Resolve<IMessageChannel>("invalid"));
            _dispatcherSettings.InputChannel.WithDefault(_container.Resolve<IMessageChannel>("input"));
            _dispatcherSettings.NumberOfMessagesToDispatchPerSlice.WithDefault(20);
            _dispatcherSettings.DurationOfDispatchingSlice.WithDefault(new TimeSpan(0, 0, 0, 0, 500));
            _dispatcherSettings.MessageProcessorTypes.WithDefault(new System.Collections.Generic.List<Type> {typeof (FakeCommandProcessor)});

            _locator = new WindsorServiceLocator(_container);

        }

        private void ConfigureContainer()
        {
            _container = new WindsorContainer();
            _container.Register(
                Component
                    .For<IBasicRecordRepository<CommandPublicationRecord>>()
                    .Instance(new InMemoryRecordRepository<CommandPublicationRecord>()));

            _container.Register(
                Component
                    .For<IBlobStorage>()
                    .Instance(new InMemoryBlobStorage()));

            _container.Register(
                Component
                    .For<IMessageSerializer>()
                    .Instance(new JsonMessageSerializer()));

            _container.Register(
                Component
                    .For<IMessageChannel>()
                    .Instance(new InMemoryMessageChannel())
                    .Named("input"));

            _container.Register(
                Component
                    .For<IMessageChannel>()
                    .Instance(new InMemoryMessageChannel())
                    .Named("invalid"));
        }

        [Test]
        public void CommandHostStarts()
        {
            var host = new CommandHost(_locator, _dispatcherSettings);

            host.Start();

            Assert.AreEqual(HostedServiceState.Started, host.State);
            //container.Register<
        }

        [Test]
        public void CommandHostStops()
        {
            var host = new CommandHost(_locator, _dispatcherSettings);

            host.Start();

            Assert.AreEqual(HostedServiceState.Started, host.State);
        }

        [Test]
        public void CommandHostDispatches()
        {
            var processor = new FakeCommandProcessor();

            _container.Register(
                Component
                    .For<IMessageProcessor<FakeCommand>>()
                    .ImplementedBy(typeof (FakeCommandProcessor)));

            _locator=  new WindsorServiceLocator(_container);

            var host = new CommandHost(_locator, _dispatcherSettings);

            host.Start();

            var channel = _container.Resolve<IMessageChannel>("input");

            var invalid = _container.Resolve<IMessageChannel>("invalid");

            var registry = GetRegistry();

            channel.Open();

            var recordOfCommandOne = registry.CreateRecord(new FakeCommand());

            channel.Send(recordOfCommandOne);

            Thread.Sleep(2000);

            Assert.Null(invalid.ReceiveSingle(TimeSpan.MaxValue));

            Assert.Greater(0, FakeCommandProcessor.FakeCommandCount);

            var recordOfCommandTwo = registry.CreateRecord(new FakeCommand2());

            channel.Send(recordOfCommandTwo);

            Thread.Sleep(2000);

            Assert.Null(invalid.ReceiveSingle(TimeSpan.MaxValue));
        
            Assert.Greater(0, FakeCommandProcessor.FakeCommandTwoCount);
        }

        private ICommandRegistry GetRegistry()
        {
            var repo = _locator.GetInstance<IBasicRecordRepository<CommandPublicationRecord>>();
            var blob = _locator.GetInstance<IBlobStorage>();
            var serializer = _locator.GetInstance<IMessageSerializer>();

            return new CommandRegistry(repo, blob, serializer);
        }
    }
}
