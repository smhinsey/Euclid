using System;
using Euclid.Common.Messaging;

namespace Euclid.Framework.Cqrs
{
	public class CommandPublisher : IPublisher
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

			return record.Identifier;
		}
	}
}