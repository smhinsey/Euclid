﻿using System;
using System.Collections.Generic;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Euclid.Common.Serialization;
using Euclid.Common.Storage.Blob;
using Euclid.Common.Storage.Record;
using Euclid.Common.TestingFakes.Registry;
using Euclid.Common.TestingFakes.Transport;
using Euclid.Common.Transport;
using NUnit.Framework;
using FakeMessage = Euclid.Common.TestingFakes.Transport.FakeMessage;

namespace Euclid.Common.UnitTests.Transport
{
    public class MessageDispatcherTests
    {
        private FakeDispatcher _dispatcher;
        private InMemoryMessageTransport _transport;

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

            var registry = new FakeRegistry(new InMemoryRecordRepository<FakeRecord>(), new InMemoryBlobStorage(), new JsonMessageSerializer());

            _dispatcher = new FakeDispatcher(container, registry);

            _transport = new InMemoryMessageTransport();
        }


        [Test]
        public void DispatchesMessage()
        {
            var settings = new MessageDispatcherSettings();
            var message = new FakeMessage();
            
            settings.InputTransport.WithDefault(new InMemoryMessageTransport());
            settings.MessageProcessorTypes.WithDefault(new List<Type> {typeof (FakeMessageProcessor)});
            settings.DurationOfDispatchingSlice.WithDefault(TimeSpan.Parse("00:00:30"));
            settings.NumberOfMessagesToDispatchPerSlice.WithDefault(30);

            _dispatcher.Configure(settings);

            _dispatcher.Enable();

            _transport.Send(message);

            Assert.AreEqual(MessageDispatcherState.Enabled, _dispatcher.State);

            _dispatcher.Disable();

            Assert.AreEqual(MessageDispatcherState.Disabled, _dispatcher.State);
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

            settings.InputTransport.WithDefault(new InMemoryMessageTransport());
            settings.MessageProcessorTypes.WithDefault(new List<Type> {typeof (FakeMessageProcessor)});
            settings.DurationOfDispatchingSlice.WithDefault(TimeSpan.Parse("00:00:30"));
            settings.NumberOfMessagesToDispatchPerSlice.WithDefault(30);

            _dispatcher.Configure(settings);

            _dispatcher.Enable();

            Assert.AreEqual(MessageDispatcherState.Enabled, _dispatcher.State);
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

            settings.InputTransport.WithDefault(new InMemoryMessageTransport());

            _dispatcher.Configure(settings);
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

            _dispatcher.Configure(settings);
        }

        [Test]
        [ExpectedException(typeof (NoDispatchingSliceDurationConfiguredException))]
        public void ThrowsWithMissingSliceDuration()
        {
            var container = new WindsorContainer();
            var settings = new MessageDispatcherSettings();

            settings.InputTransport.WithDefault(new InMemoryMessageTransport());
            settings.MessageProcessorTypes.WithDefault(new List<Type> {typeof (FakeMessageProcessor)});

            _dispatcher.Configure(settings);
        }
    }
}