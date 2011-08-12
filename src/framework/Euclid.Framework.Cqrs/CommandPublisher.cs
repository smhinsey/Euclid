using System;
using Euclid.Common.Logging;
using Euclid.Common.Messaging;

namespace Euclid.Framework.Cqrs
{
	// SELF: why is this almost identical to DefaultPublisher? Surely there is a better way to do this
	public class CommandPublisher : IPublisher, ILoggingSource
	{
		private readonly IMessageChannel _channel;
		private readonly ICommandRegistry _registry;

		public CommandPublisher(ICommandRegistry registry, IMessageChannel channel)
		{
			_registry = registry;
			_channel = channel;
		}

		public Guid PublishMessage(IMessage message)
		{
			if (_channel.State != ChannelState.Open)
			{
				_channel.Open();
			}

			var record = _registry.CreateRecord(message);

			_channel.Send(record);

			this.WriteInfoMessage("Published command {1}/{0}", record.Identifier, record.MessageType.Name);

			return record.Identifier;
		}
	}
}