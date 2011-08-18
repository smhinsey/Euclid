using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using CommonServiceLocator.WindsorAdapter;
using Euclid.Common.Messaging;
using Euclid.Common.Storage;
using Euclid.Common.Storage.Binary;
using Euclid.Common.Storage.NHibernate;
using Euclid.Common.Storage.Record;
using Euclid.Common.TestingFakes.Registry;
using Euclid.Common.TestingFakes.Transport;
using Euclid.TestingSupport;
using NHibernate;
using NUnit.Framework;
using FakeMessage = Euclid.Common.TestingFakes.Transport.FakeMessage;

namespace Euclid.Common.IntegrationTests
{
	[TestFixture]
	[Category(TestCategories.Integration)]
	public class MessageDispatcherTests : NhTestFixture<DefaultEnvelope, DefaultRecord>
	{
		public MessageDispatcherTests() : base(new AutoMapperConfiguration(typeof(DefaultEnvelope)))
		{
		}

		[SetUp]
		public override void SetUp()
		{
			base.SetUp();

			var container = new WindsorContainer();

			container.Register(
			                   Component.For<ISession>()
			                   	.UsingFactoryMethod(() => SessionFactory.OpenSession()).LifeStyle.Singleton);

			var processor = new FakeMessageProcessor();
			container.Register
				(
				 Component.For<FakeMessageProcessor>()
				 	.Instance(processor)
				);

			container.Register(Component.For<IPublicationRegistry<IEnvelope, IEnvelope>>().ImplementedBy<FakeRegistry>());
			container.Register(Component.For<IRecordMapper<DefaultEnvelope>>().ImplementedBy<NhRecordMapper<DefaultEnvelope>>());
			container.Register(Component.For<IBlobStorage>().ImplementedBy<InMemoryBlobStorage>());
			container.Register(Component.For<IMessageSerializer>().ImplementedBy<JsonMessageSerializer>());

			container.Register(Component.For<FakeMessageProcessor2>().Instance(new FakeMessageProcessor2()));

			_registry = new FakeRegistry(new NhRecordMapper<DefaultEnvelope>(SessionFactory.OpenSession()), new InMemoryBlobStorage(), new JsonMessageSerializer());

			var locator = new WindsorServiceLocator(container);

			_dispatcher = new MultitaskingMessageDispatcher<IPublicationRegistry<IEnvelope, IEnvelope>>(locator, _registry);

			_transport = new InMemoryMessageChannel();
		}

		private MultitaskingMessageDispatcher<IPublicationRegistry<IEnvelope, IEnvelope>> _dispatcher;
		private FakeRegistry _registry;
		private InMemoryMessageChannel _transport;

		private IPublicationRecord getRecord()
		{
			var msg = new FakeMessage
			          	{
			          		Created = DateTime.Now,
			          		Field1 = 1,
			          		CreatedBy = new Guid("1ABA1517-6A7B-410B-8E90-0F8C73886B01"),
			          		Field2 = new List<string>
			          		         	{
			          		         		"foo",
			          		         		"bar",
			          		         		"baz"
			          		         	}
			          	};

			var envelope = new DefaultEnvelope(msg);

			var record = (IPublicationRecord) _registry.RegisterMessage(envelope).Payload;

			return record;
		}

		[Test]
		public void DispatchesMessage()
		{
			var settings = new MessageDispatcherSettings();

			settings.InputChannel.WithDefault(new InMemoryMessageChannel());
			settings.InvalidChannel.WithDefault(new InMemoryMessageChannel());
			settings.MessageProcessorTypes.WithDefault(new List<Type> {typeof (FakeMessageProcessor)});
			settings.DurationOfDispatchingSlice.WithDefault(new TimeSpan(0, 0, 0, 0, 200));
			settings.NumberOfMessagesToDispatchPerSlice.WithDefault(30);

			_dispatcher.Configure(settings);

			_dispatcher.Enable();

			_transport.Open();

			_transport.Send(getRecord());

			Assert.AreEqual(MessageDispatcherState.Enabled, _dispatcher.State);

			Thread.Sleep(5000); // wait for message to be processed

			Assert.IsTrue(FakeMessageProcessor.ProcessedAnyMessages);

			_dispatcher.Disable();

			Assert.AreEqual(MessageDispatcherState.Disabled, _dispatcher.State);

			Assert.NotNull(_dispatcher);
		}

		[Test]
		public void DispatchesMessages()
		{
			const int numberOfMessages = 1000;

			var settings = new MessageDispatcherSettings();

			settings.InputChannel.WithDefault(new InMemoryMessageChannel());
			settings.InvalidChannel.WithDefault(new InMemoryMessageChannel());
			settings.MessageProcessorTypes.WithDefault(new List<Type> {typeof (FakeMessageProcessor)});
			settings.DurationOfDispatchingSlice.WithDefault(new TimeSpan(0, 0, 0, 0, 200));
			settings.NumberOfMessagesToDispatchPerSlice.WithDefault(30);

			_dispatcher.Configure(settings);

			_dispatcher.Enable();

			Assert.AreEqual(MessageDispatcherState.Enabled, _dispatcher.State);

			_transport.Open();

			var recordIds = new ConcurrentBag<Guid>();

			var start = DateTime.Now;

			for (var j = 0; j < numberOfMessages; j++)
			{
				var record = getRecord();
				_transport.Send(record);
				recordIds.Add(record.Identifier);
			}

			Console.WriteLine("Sent 1000 messages in {0} ms", (DateTime.Now - start).TotalMilliseconds);

			Console.WriteLine("Waiting for messages to be processed");

			start = DateTime.Now;

			Assert.AreEqual(numberOfMessages, recordIds.Count);

			var numberOfMessagesProcessed = 0;

			do
			{
				Thread.Sleep(200);

				numberOfMessagesProcessed = recordIds.Where(id =>
				                                            	{
				                                            		var envelope = _registry.GetMessage(id);

																												var record = (IPublicationRecord) envelope.Payload;

																												return record.Completed;
				                                            	}).Count();

				Console.WriteLine("{0} messages processed", numberOfMessagesProcessed);
			} while (numberOfMessagesProcessed < recordIds.Count());

			Console.WriteLine("Completed in {0} seconds", (DateTime.Now - start).TotalSeconds);

			_dispatcher.Disable();

			Assert.AreEqual(MessageDispatcherState.Disabled, _dispatcher.State);

			Assert.IsTrue(FakeMessageProcessor.ProcessedAnyMessages);
		}

		[Test]
		public void DispatchesToMultipleProcessors()
		{
			var settings = new MessageDispatcherSettings();

			settings.InputChannel.WithDefault(new InMemoryMessageChannel());
			settings.InvalidChannel.WithDefault(new InMemoryMessageChannel());
			settings.MessageProcessorTypes.WithDefault(new List<Type> {typeof (FakeMessageProcessor), typeof (FakeMessageProcessor2)});
			settings.DurationOfDispatchingSlice.WithDefault(new TimeSpan(0, 0, 0, 0, 200));
			settings.NumberOfMessagesToDispatchPerSlice.WithDefault(30);

			_dispatcher.Configure(settings);

			_dispatcher.Enable();

			settings.InputChannel.Value.Open();

			settings.InvalidChannel.Value.Open();

			settings.InputChannel.Value.Send(getRecord());

			Thread.Sleep(750);

			Assert.IsTrue(FakeMessageProcessor.ProcessedAnyMessages);

			Thread.Sleep(750);

			Assert.IsTrue(FakeMessageProcessor2.ProcessedAnyMessages);

			_dispatcher.Disable();

			settings.InvalidChannel.Value.Close();

			settings.InputChannel.Value.Close();
		}

		[Test]
		public void EnablesAndDisables()
		{
			var container = new WindsorContainer();
			var settings = new MessageDispatcherSettings();

			settings.InputChannel.WithDefault(new InMemoryMessageChannel());
			settings.InvalidChannel.WithDefault(new InMemoryMessageChannel());
			settings.MessageProcessorTypes.WithDefault(new List<Type> {typeof (FakeMessageProcessor)});
			settings.DurationOfDispatchingSlice.WithDefault(TimeSpan.Parse("00:00:30"));
			settings.NumberOfMessagesToDispatchPerSlice.WithDefault(30);

			_dispatcher.Configure(settings);

			_dispatcher.Enable();

			Assert.AreEqual(MessageDispatcherState.Enabled, _dispatcher.State);

			_dispatcher.Disable();

			Assert.AreEqual(MessageDispatcherState.Disabled, _dispatcher.State);
		}

		[Test]
		public void EnablesWithoutError()
		{
			var container = new WindsorContainer();
			var settings = new MessageDispatcherSettings();

			settings.InputChannel.WithDefault(new InMemoryMessageChannel());
			settings.InvalidChannel.WithDefault(new InMemoryMessageChannel());
			settings.MessageProcessorTypes.WithDefault(new List<Type> {typeof (FakeMessageProcessor)});
			settings.DurationOfDispatchingSlice.WithDefault(TimeSpan.Parse("00:00:30"));
			settings.NumberOfMessagesToDispatchPerSlice.WithDefault(30);

			_dispatcher.Configure(settings);

			_dispatcher.Enable();

			Assert.AreEqual(MessageDispatcherState.Enabled, _dispatcher.State);
		}

		[Test]
		public void ErrorsWhenChannelsArentConfigured()
		{
			var settings = new MessageDispatcherSettings();

			Assert.Throws(typeof (NoInputChannelConfiguredException), () => _dispatcher.Configure(settings));

			settings.InputChannel.WithDefault(new InMemoryMessageChannel());

			Assert.Throws(typeof (NoInputChannelConfiguredException), () => _dispatcher.Configure(settings));

			settings.InvalidChannel.WithDefault(new InMemoryMessageChannel());

			Assert.Throws(typeof (NoMessageProcessorsConfiguredException), () => _dispatcher.Configure(settings));

			settings.MessageProcessorTypes.WithDefault(new List<Type> {typeof (FakeMessageProcessor)});

			_dispatcher.Configure(settings);
		}

		[Test]
		public void MessagesThatArentPublicationRecordsAreInvalid()
		{
			var settings = new MessageDispatcherSettings();

			settings.InputChannel.WithDefault(new InMemoryMessageChannel());
			settings.InvalidChannel.WithDefault(new InMemoryMessageChannel());
			settings.MessageProcessorTypes.WithDefault(new List<Type> {typeof (FakeMessageProcessor)});
			settings.DurationOfDispatchingSlice.WithDefault(new TimeSpan(0, 0, 0, 0, 200));
			settings.NumberOfMessagesToDispatchPerSlice.WithDefault(30);

			_dispatcher.Configure(settings);

			_dispatcher.Enable();

			settings.InputChannel.Value.Open();

			settings.InvalidChannel.Value.Open();

			settings.InputChannel.Value.Send(new FakeMessage());

			Thread.Sleep(750);

			var received = settings.InvalidChannel.Value.ReceiveSingle(TimeSpan.MaxValue);

			Assert.NotNull(received);

			Assert.AreEqual(typeof (FakeMessage), received.GetType());

			_dispatcher.Disable();

			settings.InvalidChannel.Value.Close();

			settings.InputChannel.Value.Close();
		}

		[Test]
		[ExpectedException(typeof (NoInputChannelConfiguredException))]
		public void ThrowsWithMissingInputTransport()
		{
			var container = new WindsorContainer();
			var settings = new MessageDispatcherSettings();

			_dispatcher.Configure(settings);
		}

		[Test]
		[ExpectedException(typeof (NoMessageProcessorsConfiguredException))]
		public void ThrowsWithMissingMessageProcessors()
		{
			var container = new WindsorContainer();
			var settings = new MessageDispatcherSettings();

			settings.InvalidChannel.WithDefault(new InMemoryMessageChannel());
			settings.InputChannel.WithDefault(new InMemoryMessageChannel());

			_dispatcher.Configure(settings);
		}
	}
}