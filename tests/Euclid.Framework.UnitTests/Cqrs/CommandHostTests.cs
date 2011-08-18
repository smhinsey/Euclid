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
using NUnit.Framework;
using log4net.Config;

namespace Euclid.Framework.UnitTests.Cqrs
{
	[TestFixture]
	[Category(TestCategories.Unit)]
	public class CommandHostTests
	{
		#region Setup/Teardown

		[SetUp]
		public void Setup()
		{
			BasicConfigurator.Configure();

			ConfigureContainer();

			_dispatcherSettings = new MessageDispatcherSettings();

			_dispatcherSettings.InvalidChannel.WithDefault(_container.Resolve<IMessageChannel>("invalid"));
			_dispatcherSettings.InputChannel.WithDefault(_container.Resolve<IMessageChannel>("input"));
			_dispatcherSettings.NumberOfMessagesToDispatchPerSlice.WithDefault(20);
			_dispatcherSettings.DurationOfDispatchingSlice.WithDefault(new TimeSpan(0, 0, 0, 0, 500));
			_dispatcherSettings.MessageProcessorTypes.WithDefault(new List<Type> {typeof (FakeCommandProcessor)});

			_locator = new WindsorServiceLocator(_container);
		}

		#endregion

		private IMessageDispatcherSettings _dispatcherSettings;
		private IWindsorContainer _container;
		private WindsorServiceLocator _locator;
		//private IList<ICommandDispatcher> _dispatchers = new List<ICommandDispatcher>();

		private void ConfigureContainer()
		{
			_container = new WindsorContainer();
			_container.Register
				(
				 Component
				 	.For<IRecordMapper<CommandPublicationRecord>>()
				 	.Instance(new InMemoryRecordMapper<CommandPublicationRecord>()));

			_container.Register
				(
				 Component
				 	.For<IBlobStorage>()
				 	.Instance(new InMemoryBlobStorage()));

			_container.Register
				(
				 Component
				 	.For<IMessageSerializer>()
				 	.Instance(new JsonMessageSerializer()));

			_container.Register
				(
				 Component
				 	.For<IMessageChannel>()
				 	.Instance(new InMemoryMessageChannel())
				 	.Named("input"));

			_container.Register
				(
				 Component
				 	.For<IMessageChannel>()
				 	.Instance(new InMemoryMessageChannel())
				 	.Named("invalid"));

			_container.Register
				(
				 Component
				 	.For<FakeCommandProcessor>()
				 	.ImplementedBy(typeof (FakeCommandProcessor)));
		}

		private CommandHost GetCommandHost()
		{
			var dispatchers = new List<ICommandDispatcher>();
			var dispatcher = new CommandDispatcher(_locator, GetRegistry());

			dispatcher.Configure(_dispatcherSettings);

			dispatchers.Add(dispatcher);

			return new CommandHost(dispatchers);
		}

		private ICommandRegistry GetRegistry()
		{
			var repo = _locator.GetInstance<IRecordMapper<CommandPublicationRecord>>();
			var blob = _locator.GetInstance<IBlobStorage>();
			var serializer = _locator.GetInstance<IMessageSerializer>();

			return new CommandRegistry(repo, blob, serializer);
		}

		[Test]
		public void CommandHostCancel()
		{
			var host = GetCommandHost();

			host.Cancel();

			Thread.Sleep(250);

			Assert.AreEqual(HostedServiceState.Stopped, host.State);
		}

		[Test]
		public void CommandHostDispatches()
		{
			var host = GetCommandHost();

			host.Start();

			var channel = _container.Resolve<IMessageChannel>("input");

			var invalid = _container.Resolve<IMessageChannel>("invalid");

			var registry = GetRegistry();

			channel.Open();

			var recordOfCommandOne = registry.CreateRecord(new FakeCommand());

			channel.Send(recordOfCommandOne);

			Thread.Sleep(750);

			Assert.Null(invalid.ReceiveSingle(TimeSpan.MaxValue));

			Assert.Greater(FakeCommandProcessor.FakeCommandCount, 0);

			var recordOfCommandTwo = registry.CreateRecord(new FakeCommand2());

			channel.Send(recordOfCommandTwo);

			Thread.Sleep(750);

			Assert.Null(invalid.ReceiveSingle(TimeSpan.MaxValue));

			Assert.Greater(FakeCommandProcessor.FakeCommandTwoCount, 0);

			var recordOfCommandThree = registry.CreateRecord(new FakeCommand3());

			channel.Send(recordOfCommandThree);

			Thread.Sleep(750);

			recordOfCommandThree = registry.GetRecord(recordOfCommandThree.Identifier);

			Assert.True(recordOfCommandThree.Error);

			Assert.False(recordOfCommandThree.Dispatched);

			Assert.True(recordOfCommandThree.Completed);
		}

		[Test]
		public void CommandHostStarts()
		{
			var host = GetCommandHost();

			host.Start();

			Assert.AreEqual(HostedServiceState.Started, host.State);
			//container.Register<
		}

		[Test]
		public void TestFluentConfiguration()
		{
			var c = CommandHostService.Configure()
				.AddDispatcher
				(
				 Dispatcher.Configure()
				 	.AddCommandProcessor<FakeCommandProcessor>()
				 	.InputChannelAs<InMemoryMessageChannel>()
				 	.InvalidChannelAs<InMemoryMessageChannel>()
				 	.BlobStorageAs<InMemoryBlobStorage>()
				 	.RecordRepositoryAs<InMemoryRecordMapper<CommandPublicationRecord>>()
				 	.CommandSerializerAs<JsonMessageSerializer>()
				 	.PollingInterval().Milliseconds(50)
				 	.ProcessMessageInBatchesOf(25))
				.GetCommandHost();

			Assert.NotNull(c);

			Assert.AreEqual(typeof (CommandHost), c.GetType());

			c.Start();

			Assert.AreEqual(HostedServiceState.Started, c.State);

			c.Cancel();

			Assert.AreEqual(HostedServiceState.Stopped, c.State);
		}
	}
}