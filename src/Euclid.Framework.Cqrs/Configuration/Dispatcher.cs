using System;
using System.Collections.Generic;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using CommonServiceLocator.WindsorAdapter;
using Euclid.Common.Messaging;
using Euclid.Common.Storage;

namespace Euclid.Framework.Cqrs.Configuration
{
    public class Dispatcher
    {
        private int _bs;
        private readonly TimeSpanConfiguration<Dispatcher> _tsc;
        private readonly IWindsorContainer _container;
        private readonly IList<Type> _messageProcessors;
        
        public static Dispatcher Configure()
        {
            return new Dispatcher();
        }

        private Dispatcher()
        {
            _tsc = new TimeSpanConfiguration<Dispatcher>(this);
            _container = new WindsorContainer();
            _messageProcessors = new List<Type>();
        }

        public Dispatcher BlobStorageAs<T>() where T : IBlobStorage
        {
            _container.Register(
                Component
                    .For<IBlobStorage>()
                    .ImplementedBy(typeof (T)));

            return this;
        }

        public Dispatcher RecordRepositoryAs<T>() where T : IBasicRecordRepository<CommandPublicationRecord>
        {
            _container.Register(
                Component
                    .For<IBasicRecordRepository<CommandPublicationRecord>>()
                    .ImplementedBy(typeof (T)));

            return this;
        }

        public Dispatcher CommandSerializerAs<T>() where T : IMessageSerializer
        {
            _container.Register(
                Component
                    .For<IMessageSerializer>()
                    .ImplementedBy(typeof (T)));

            return this;
        }

        public Dispatcher InputChannelAs<T>() where T : IMessageChannel
        {
            _container.Register(
                Component
                    .For<IMessageChannel>()
                    .ImplementedBy(typeof (T))
                    .Named("input"));

            return this;
        }

        public Dispatcher InvalidChannelAs<T>() where T : IMessageChannel
        {
            _container.Register(
                Component
                    .For<IMessageChannel>()
                    .ImplementedBy(typeof(T))
                    .Named("invalid"));

            return this;
        }

        public Dispatcher AddCommandProcessor<T>() where T : IMessageProcessor
        {
            _container.Register(
                Component
                    .For(typeof(T))
                    .ImplementedBy(typeof (T)));

            _messageProcessors.Add(typeof(T));

            return this;
        }

        public TimeSpanConfiguration<Dispatcher> PollingInterval()
        {
            return _tsc;
        }

        public Dispatcher ProcessMessageInBatchesOf(int batchSize)
        {
            _bs = batchSize;

            return this;
        }

        public static ICommandDispatcher GetConfiguredCommandDispatcher(Dispatcher config)
        {
            var settings = new MessageDispatcherSettings();
            settings.InvalidChannel.WithDefault(config._container.Resolve<IMessageChannel>("invalid"));
            settings.InputChannel.WithDefault(config._container.Resolve<IMessageChannel>("input"));
            settings.NumberOfMessagesToDispatchPerSlice.WithDefault(config._bs);
            settings.DurationOfDispatchingSlice.WithDefault((TimeSpan)config._tsc);
            settings.MessageProcessorTypes.WithDefault(config._messageProcessors);

            var locator = new WindsorServiceLocator(config._container);

            var repo = locator.GetInstance<IBasicRecordRepository<CommandPublicationRecord>>();
            var blob = locator.GetInstance<IBlobStorage>();
            var serializer = locator.GetInstance<IMessageSerializer>();

            var registry = new CommandRegistry(repo, blob, serializer);

            var dispatcher = new CommandDispatcher(locator, registry);
            dispatcher.Configure(settings);

            return dispatcher;
        }
    }
}