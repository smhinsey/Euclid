using System;
using System.Collections.Generic;
using System.Threading;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using CommonServiceLocator.WindsorAdapter;
using Euclid.Common.Messaging;
using Euclid.Common.ServiceHost;
using Euclid.Common.Storage;
using Euclid.Common.Storage.Binary;
using Euclid.Common.Storage.Record;
using Euclid.Framework.Cqrs;
using Euclid.Framework.Cqrs.Settings;
using Euclid.Framework.TestingFakes.Cqrs;
using Euclid.TestingSupport;
using log4net.Config;
using NUnit.Framework;

namespace Euclid.Framework.UnitTests.Cqrs
{
	[TestFixture]
	[Category(TestCategories.Unit)]
	public class CommandHostTests
	{
		private IWindsorContainer _container;

		private IMessageDispatcherSettings _dispatcherSettings;

		private WindsorServiceLocator _locator;

		// private IList<ICommandDispatcher> _dispatchers = new List<ICommandDispatcher>();

		[Test]
		public void CommandHostCancel()
		{
			var host = this.GetCommandHost();

			host.Cancel();
			Thread.Sleep(250);
			Assert.AreEqual(HostedServiceState.Stopped, host.State);
		}

		[Test]
		public void CommandHostDispatches()
		{
			var host = this.GetCommandHost();

			host.Start();

			var channel = this._container.Resolve<IMessageChannel>("input");

			var invalid = this._container.Resolve<IMessageChannel>("invalid");

			var registry = this.GetRegistry();

			channel.Open();

			var recordOfCommandOne = registry.PublishMessage(new FakeCommand());

			channel.Send(recordOfCommandOne);

			Thread.Sleep(750);

			Assert.Null(invalid.ReceiveSingle(TimeSpan.MaxValue));

			Assert.Greater(FakeCommandProcessor.FakeCommandCount, 0);

			var recordOfCommandTwo = registry.PublishMessage(new FakeCommand2());

			channel.Send(recordOfCommandTwo);

			Thread.Sleep(750);

			Assert.Null(invalid.ReceiveSingle(TimeSpan.MaxValue));

			Assert.Greater(FakeCommandProcessor.FakeCommandTwoCount, 0);

			var recordOfCommandThree = registry.PublishMessage(new FakeCommand3());

			channel.Send(recordOfCommandThree);

			Thread.Sleep(750);

			recordOfCommandThree = registry.GetPublicationRecord(recordOfCommandThree.Identifier);

			Assert.True(recordOfCommandThree.Error);

			Assert.False(recordOfCommandThree.Dispatched);

			Assert.True(recordOfCommandThree.Completed);
		}

		[Test]
		public void CommandHostStarts()
		{
			var host = this.GetCommandHost();

			host.Start();

			Assert.AreEqual(HostedServiceState.Started, host.State);

			// container.Register<
		}

		[SetUp]
		public void Setup()
		{
			BasicConfigurator.Configure();

			this.ConfigureContainer();

			this._dispatcherSettings = new MessageDispatcherSettings();

			this._dispatcherSettings.InvalidChannel.WithDefault(this._container.Resolve<IMessageChannel>("invalid"));
			this._dispatcherSettings.InputChannel.WithDefault(this._container.Resolve<IMessageChannel>("input"));
			this._dispatcherSettings.NumberOfMessagesToDispatchPerSlice.WithDefault(20);
			this._dispatcherSettings.DurationOfDispatchingSlice.WithDefault(new TimeSpan(0, 0, 0, 0, 500));
			this._dispatcherSettings.MessageProcessorTypes.WithDefault(new List<Type> { typeof(FakeCommandProcessor) });

			this._locator = new WindsorServiceLocator(this._container);
		}

		[Test]
		public void TestFluentConfiguration()
		{
			var c =
				CommandHostService.Configure().AddDispatcher(
					Dispatcher.Configure().AddCommandProcessor<FakeCommandProcessor>().InputChannelAs<InMemoryMessageChannel>().
						InvalidChannelAs<InMemoryMessageChannel>().BlobStorageAs<InMemoryBlobStorage>().RecordRepositoryAs
						<InMemoryRecordMapper<CommandPublicationRecord>>().CommandSerializerAs<JsonMessageSerializer>().PollingInterval().
						Milliseconds(50).ProcessMessageInBatchesOf(25)).GetCommandHost();

			Assert.NotNull(c);

			Assert.AreEqual(typeof(CommandHost), c.GetType());

			c.Start();

			Assert.AreEqual(HostedServiceState.Started, c.State);

			c.Cancel();

			Assert.AreEqual(HostedServiceState.Stopped, c.State);
		}

		private void ConfigureContainer()
		{
			this._container = new WindsorContainer();
			this._container.Register(
				Component.For<IRecordMapper<CommandPublicationRecord>>().Instance(
					new InMemoryRecordMapper<CommandPublicationRecord>()));

			this._container.Register(Component.For<IBlobStorage>().Instance(new InMemoryBlobStorage()));

			this._container.Register(Component.For<IMessageSerializer>().Instance(new JsonMessageSerializer()));

			this._container.Register(Component.For<IMessageChannel>().Instance(new InMemoryMessageChannel()).Named("input"));

			this._container.Register(Component.For<IMessageChannel>().Instance(new InMemoryMessageChannel()).Named("invalid"));

			this._container.Register(Component.For<FakeCommandProcessor>().ImplementedBy(typeof(FakeCommandProcessor)));
		}

		private CommandHost GetCommandHost()
		{
			var dispatchers = new List<ICommandDispatcher>();
			var dispatcher = new CommandDispatcher(this._locator, this.GetRegistry());

			dispatcher.Configure(this._dispatcherSettings);

			dispatchers.Add(dispatcher);

			return new CommandHost(dispatchers);
		}

		private ICommandRegistry GetRegistry()
		{
			var repo = this._locator.GetInstance<IRecordMapper<CommandPublicationRecord>>();
			var blob = this._locator.GetInstance<IBlobStorage>();
			var serializer = this._locator.GetInstance<IMessageSerializer>();

			return new CommandRegistry(repo, blob, serializer);
		}
	}
}