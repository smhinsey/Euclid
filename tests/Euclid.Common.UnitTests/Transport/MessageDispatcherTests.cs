using System;
using System.Collections.Generic;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Euclid.Common.TestingFakes.Transport;
using Euclid.Common.Transport;
using NUnit.Framework;

namespace Euclid.Common.UnitTests.Transport
{
	public class MessageDispatcherTests
	{
		[Test]
		public void DispatchesMessage()
		{
			var container = new WindsorContainer();
			var processor = new FakeMessageProcessor();
			var settings = new MessageDispatcherSettings();
			var transport = new InMemoryMessageTransport();
			var message = new FakeMessage();

			container.Register
				(
				 Component.For<FakeMessageProcessor>()
				 	.Instance(processor)
				);

			settings.InputTransport.WithDefault(new InMemoryMessageTransport());
			settings.MessageProcessorTypes.WithDefault(new List<Type> {typeof (FakeMessageProcessor)});
			settings.DurationOfDispatchingSlice.WithDefault(TimeSpan.Parse("00:00:30"));
			settings.NumberOfMessagesToDispatchPerSlice.WithDefault(30);

			var dispatcher = new MultitaskingMessageDispatcher(container);

			dispatcher.Configure(settings);

			dispatcher.Enable();

			transport.Send(message);

			Assert.AreEqual(MessageDispatcherState.Enabled, dispatcher.State);

			dispatcher.Disable();

			Assert.AreEqual(MessageDispatcherState.Disabled, dispatcher.State);
			Assert.IsTrue(FakeMessageProcessor.ProcessedAnyMessages);
		}

		[Test]
		public void EnablesAndDisables()
		{
			var container = new WindsorContainer();
			var settings = new MessageDispatcherSettings();

			settings.InputTransport.WithDefault(new InMemoryMessageTransport());
			settings.MessageProcessorTypes.WithDefault(new List<Type> {typeof (FakeMessageProcessor)});
			settings.DurationOfDispatchingSlice.WithDefault(TimeSpan.Parse("00:00:30"));
			settings.NumberOfMessagesToDispatchPerSlice.WithDefault(30);

			var dispatcher = new MultitaskingMessageDispatcher(container);

			dispatcher.Configure(settings);

			dispatcher.Enable();

			Assert.AreEqual(MessageDispatcherState.Enabled, dispatcher.State);

			dispatcher.Disable();

			Assert.AreEqual(MessageDispatcherState.Disabled, dispatcher.State);
		}

		[Test]
		public void EnablesWithoutError()
		{
			var container = new WindsorContainer();
			var settings = new MessageDispatcherSettings();

			settings.InputTransport.WithDefault(new InMemoryMessageTransport());
			settings.MessageProcessorTypes.WithDefault(new List<Type> {typeof (FakeMessageProcessor)});
			settings.DurationOfDispatchingSlice.WithDefault(TimeSpan.Parse("00:00:30"));
			settings.NumberOfMessagesToDispatchPerSlice.WithDefault(30);

			var dispatcher = new MultitaskingMessageDispatcher(container);

			dispatcher.Configure(settings);

			dispatcher.Enable();

			Assert.AreEqual(MessageDispatcherState.Enabled, dispatcher.State);
		}

		[Test]
		[ExpectedException(typeof (NoInputTransportConfiguredException))]
		public void ThrowsWithMissingInputTransport()
		{
			var container = new WindsorContainer();
			var settings = new MessageDispatcherSettings();

			var dispatcher = new MultitaskingMessageDispatcher(container);

			dispatcher.Configure(settings);
		}

		[Test]
		[ExpectedException(typeof (NoMessageProcessorsConfiguredException))]
		public void ThrowsWithMissingMessageProcessors()
		{
			var container = new WindsorContainer();
			var settings = new MessageDispatcherSettings();

			settings.InputTransport.WithDefault(new InMemoryMessageTransport());

			var dispatcher = new MultitaskingMessageDispatcher(container);

			dispatcher.Configure(settings);
		}

		[Test]
		[ExpectedException(typeof (NoNumberOfMessagesPerSliceConfiguredException))]
		public void ThrowsWithMissingMessagesPerSliceSetting()
		{
			var container = new WindsorContainer();
			var settings = new MessageDispatcherSettings();

			settings.InputTransport.WithDefault(new InMemoryMessageTransport());
			settings.MessageProcessorTypes.WithDefault(new List<Type> {typeof (FakeMessageProcessor)});
			settings.DurationOfDispatchingSlice.WithDefault(TimeSpan.Parse("00:00:30"));

			var dispatcher = new MultitaskingMessageDispatcher(container);

			dispatcher.Configure(settings);
		}

		[Test]
		[ExpectedException(typeof (NoDispatchingSliceDurationConfiguredException))]
		public void ThrowsWithMissingSliceDuration()
		{
			var container = new WindsorContainer();
			var settings = new MessageDispatcherSettings();

			settings.InputTransport.WithDefault(new InMemoryMessageTransport());
			settings.MessageProcessorTypes.WithDefault(new List<Type> {typeof (FakeMessageProcessor)});

			var dispatcher = new MultitaskingMessageDispatcher(container);

			dispatcher.Configure(settings);
		}
	}
}