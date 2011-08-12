using System;
using System.Collections.Generic;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using CommonServiceLocator.WindsorAdapter;
using Euclid.Common.Messaging;
using Euclid.Common.Storage.Binary;
using Euclid.Common.Storage.Record;

namespace Euclid.Framework.Cqrs.Configuration
{
	public class Dispatcher
	{
		private readonly IWindsorContainer _container;
		private readonly IList<Type> _messageProcessors;
		private readonly TimeSpanConfiguration<Dispatcher> _tsc;
		private int _bs = 25;

		private bool _hasBlob;
		private bool _hasInput;
		private bool _hasInvalid;
		private bool _hasRepo;
		private bool _hasSerializer;

		private Dispatcher()
		{
			_tsc = new TimeSpanConfiguration<Dispatcher>(this);
			_container = new WindsorContainer();
			_messageProcessors = new List<Type>();
		}

		public Dispatcher AddCommandProcessor<T>() where T : IMessageProcessor
		{
			_container.Register
				(
				 Component
				 	.For(typeof (T))
				 	.ImplementedBy(typeof (T)));

			_messageProcessors.Add(typeof (T));

			return this;
		}

		public Dispatcher BlobStorageAs<T>() where T : IBlobStorage
		{
			_container.Register
				(
				 Component
				 	.For<IBlobStorage>()
				 	.ImplementedBy(typeof (T)));

			_hasBlob = true;

			return this;
		}

		public Dispatcher CommandSerializerAs<T>() where T : IMessageSerializer
		{
			_container.Register
				(
				 Component
				 	.For<IMessageSerializer>()
				 	.ImplementedBy(typeof (T)));

			_hasSerializer = true;

			return this;
		}

		public Dispatcher InputChannelAs<T>() where T : IMessageChannel
		{
			_container.Register
				(
				 Component
				 	.For<IMessageChannel>()
				 	.ImplementedBy(typeof (T))
				 	.Named("input"));

			_hasInput = true;

			return this;
		}

		public Dispatcher InvalidChannelAs<T>() where T : IMessageChannel
		{
			_container.Register
				(
				 Component
				 	.For<IMessageChannel>()
				 	.ImplementedBy(typeof (T))
				 	.Named("invalid"));

			_hasInvalid = true;

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

		public Dispatcher RecordRepositoryAs<T>() where T : IRecordMapper<CommandPublicationRecord>
		{
			_container.Register
				(
				 Component
				 	.For<IRecordMapper<CommandPublicationRecord>>()
				 	.ImplementedBy(typeof (T)));

			_hasRepo = true;

			return this;
		}

		public static Dispatcher Configure()
		{
			return new Dispatcher();
		}

		public static ICommandDispatcher GetConfiguredCommandDispatcher(Dispatcher config)
		{
			EnsureDispatcherConfiguration(config);

			var dispatchInterval = config._tsc == null ? new TimeSpan(0, 0, 0, 0, 1500) : (TimeSpan) config._tsc;

			var settings = new MessageDispatcherSettings();
			settings.InvalidChannel.WithDefault(config._container.Resolve<IMessageChannel>("invalid"));
			settings.InputChannel.WithDefault(config._container.Resolve<IMessageChannel>("input"));
			settings.NumberOfMessagesToDispatchPerSlice.WithDefault(config._bs);
			settings.DurationOfDispatchingSlice.WithDefault(dispatchInterval);
			settings.MessageProcessorTypes.WithDefault(config._messageProcessors);

			var locator = new WindsorServiceLocator(config._container);

			var repo = locator.GetInstance<IRecordMapper<CommandPublicationRecord>>();
			var blob = locator.GetInstance<IBlobStorage>();
			var serializer = locator.GetInstance<IMessageSerializer>();

			var registry = new CommandRegistry(repo, blob, serializer);

			var dispatcher = new CommandDispatcher(locator, registry);
			dispatcher.Configure(settings);

			return dispatcher;
		}

		private static void EnsureDispatcherConfiguration(Dispatcher config)
		{
			var configErrors = new List<string>();
			if (!config._hasBlob)
			{
				configErrors.Add("No BlobStorage service configured, call BlobStorageAs<T> with the appropriate IBlobStorage implementation");
			}

			if (!config._hasInput)
			{
				configErrors.Add("No InputChannel configured, call InputChannelAs<T> with the appropriate IMessageChannel implementation");
			}

			if (!config._hasInvalid)
			{
				configErrors.Add("No InvalidChannel configured, call InvalidChannelAs<T> with the appropriate IMessageChannel implementation");
			}

			if (!config._hasRepo)
			{
				configErrors.Add("No RecordRepository configured, call RecordRepositoryAs<T> with the appropriate IBasicRecordRepository<CommandPublicationRecord>");
			}

			if (!config._hasSerializer)
			{
				configErrors.Add("No MessageSerializer configured, call CommandSerializerAs<T> with the appropriate IMessageSerializer implementation");
			}

			if (config._messageProcessors.Count == 0)
			{
				configErrors.Add("No CommandProcessors configured, add one or more CommandProcessors by calling AddCommandProcessor<T> with the appropriate IMessageProcessor implementation");
			}

			if (configErrors.Count > 0)
			{
				throw new CommandDispatcherConfigurationException(configErrors);
			}
		}
	}
}