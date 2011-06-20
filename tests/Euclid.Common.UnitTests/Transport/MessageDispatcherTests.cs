using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using CommonServiceLocator.WindsorAdapter;
using Euclid.Common.Messaging;
using Euclid.Common.Messaging.Exceptions;
using Euclid.Common.Storage.Blob;
using Euclid.Common.Storage.Record;
using Euclid.Common.TestingFakes.Registry;
using Euclid.Common.TestingFakes.Transport;
using NUnit.Framework;
using FakeMessage = Euclid.Common.TestingFakes.Transport.FakeMessage;

namespace Euclid.Common.UnitTests.Transport
{
	public class MessageDispatcherTests
	{
		private FakeDispatcher _dispatcher;
		private FakeRegistry _registry;
		private InMemoryMessageChannel _transport;

		[Test]
		public void DispatchesMessage()
		{
			var settings = new MessageDispatcherSettings();

			settings.InputTransport.WithDefault(new InMemoryMessageChannel());
			settings.MessageProcessorTypes.WithDefault(new List<Type> {typeof (FakeMessageProcessor)});
			settings.DurationOfDispatchingSlice.WithDefault(new TimeSpan(0, 0, 0, 0, 200));
			settings.NumberOfMessagesToDispatchPerSlice.WithDefault(30);

			_dispatcher.Configure(settings);

			_dispatcher.Enable();

			_transport.Open();

			_transport.Send(GetRecord());

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
			var settings = new MessageDispatcherSettings();

			settings.InputTransport.WithDefault(new InMemoryMessageChannel());
			settings.MessageProcessorTypes.WithDefault(new List<Type> {typeof (FakeMessageProcessor)});
			settings.DurationOfDispatchingSlice.WithDefault(new TimeSpan(0, 0, 0, 0, 200));
			settings.NumberOfMessagesToDispatchPerSlice.WithDefault(30);

			_dispatcher.Configure(settings);

			_dispatcher.Enable();

			Assert.AreEqual(MessageDispatcherState.Enabled, _dispatcher.State);

			_transport.Open();

			var recordIds = new ConcurrentBag<Guid>();

			var start = DateTime.Now;

			var results = Parallel.For
				(0, 50, work =>
				        	{
				        		for (var j = 0; j < 100; j++)
				        		{
				        			var record = GetRecord();
				        			_transport.Send(record);
				        			recordIds.Add(record.Identifier);
				        		}
				        	});

			Console.WriteLine("Sent 5000 messages in {0} ms", (DateTime.Now - start).TotalMilliseconds);

			Console.WriteLine("Waiting for messages to be processed");

			start = DateTime.Now;

			Assert.AreEqual(5000, recordIds.Count);

			var numberOfMessagesProcessed = 0;

			do
			{
				Thread.Sleep(200);

				numberOfMessagesProcessed = recordIds.Where(id => _registry.GetRecord(id).Completed).Count();

				Console.WriteLine("{0} messages processed", numberOfMessagesProcessed);
			} while (numberOfMessagesProcessed < recordIds.Count());

			Console.WriteLine("Completed in {0} seconds", (DateTime.Now - start).TotalSeconds);

			_dispatcher.Disable();

			Assert.AreEqual(MessageDispatcherState.Disabled, _dispatcher.State);

			Assert.IsTrue(FakeMessageProcessor.ProcessedAnyMessages);
		}

		[Test]
		public void EnablesAndDisables()
		{
			var container = new WindsorContainer();
			var settings = new MessageDispatcherSettings();

			settings.InputTransport.WithDefault(new InMemoryMessageChannel());
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

			settings.InputTransport.WithDefault(new InMemoryMessageChannel());
			settings.MessageProcessorTypes.WithDefault(new List<Type> {typeof (FakeMessageProcessor)});
			settings.DurationOfDispatchingSlice.WithDefault(TimeSpan.Parse("00:00:30"));
			settings.NumberOfMessagesToDispatchPerSlice.WithDefault(30);

			_dispatcher.Configure(settings);

			_dispatcher.Enable();

			Assert.AreEqual(MessageDispatcherState.Enabled, _dispatcher.State);
		}

		[SetUp]
		public void Setup()
		{
			var container = new WindsorContainer();
			var processor = new FakeMessageProcessor();
			container.Register
				(
				 Component.For<FakeMessageProcessor>()
				 	.Instance(processor)
				);

			_registry = new FakeRegistry(new InMemoryRecordRepository<FakePublicationRecord>(), new InMemoryBlobStorage(), new JsonMessageSerializer());

			var locator = new WindsorServiceLocator(container);

			_dispatcher = new FakeDispatcher(locator, _registry);

			_transport = new InMemoryMessageChannel();
		}

		[Test]
		[ExpectedException(typeof (NoInputTransportConfiguredException))]
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

			settings.InputTransport.WithDefault(new InMemoryMessageChannel());

			_dispatcher.Configure(settings);
		}

		[Test]
		[ExpectedException(typeof (NoNumberOfMessagesPerSliceConfiguredException))]
		public void ThrowsWithMissingMessagesPerSliceSetting()
		{
			var container = new WindsorContainer();
			var settings = new MessageDispatcherSettings();

			settings.InputTransport.WithDefault(new InMemoryMessageChannel());
			settings.MessageProcessorTypes.WithDefault(new List<Type> {typeof (FakeMessageProcessor)});
			settings.DurationOfDispatchingSlice.WithDefault(TimeSpan.Parse("00:00:30"));

			_dispatcher.Configure(settings);
		}

		[Test]
		[ExpectedException(typeof (NoDispatchingSliceDurationConfiguredException))]
		public void ThrowsWithMissingSliceDuration()
		{
			var container = new WindsorContainer();
			var settings = new MessageDispatcherSettings();

			settings.InputTransport.WithDefault(new InMemoryMessageChannel());
			settings.MessageProcessorTypes.WithDefault(new List<Type> {typeof (FakeMessageProcessor)});

			_dispatcher.Configure(settings);
		}

		private IPublicationRecord GetRecord()
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

			return _registry.CreateRecord(msg);
		}
	}
}