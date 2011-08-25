using System;
using Euclid.Common.Logging;

namespace Euclid.Common.Messaging
{
	public class DefaultPublisher : IPublisher, ILoggingSource
	{
		private readonly IMessageChannel _channel;

		private readonly IPublicationRegistry<IPublicationRecord, IPublicationRecord> _publicationRegistry;

		public DefaultPublisher
			(
			IPublicationRegistry<IPublicationRecord, IPublicationRecord> publicationRegistry, IMessageChannel channel)
		{
			_publicationRegistry = publicationRegistry;
			_channel = channel;
		}

		public Guid PublishMessage(IMessage message)
		{
			if (message.Identifier == Guid.Empty)
			{
				message.Identifier = Guid.NewGuid();
			}

			if (_channel.State != ChannelState.Open)
			{
				this.WriteDebugMessage("Publication to closed channel detected. Attempting to open channel.");

				_channel.Open();

				this.WriteDebugMessage("Channel has been opened, publication may proceed.");
			}

			var record = _publicationRegistry.PublishMessage(message);

			_channel.Send(record);

			this.WriteInfoMessage(
			                      string.Format(
			                                    "Message {0} (record {1}) was successfully published via the channel {2}({3}).", 
			                                    message.GetType().Name, 
			                                    record.Identifier, 
			                                    _channel.GetType().Name, 
			                                    _channel.ChannelName));

			return record.Identifier;
		}
	}
}
