using System;
using Euclid.Common.Logging;

namespace Euclid.Common.Messaging
{
	public class DefaultPublisher : IPublisher, ILoggingSource
	{
		private readonly IMessageChannel _channel;

		private readonly IPublicationRegistry<IPublicationRecord, IPublicationRecord> _publicationRegistry;

		public DefaultPublisher(
			IPublicationRegistry<IPublicationRecord, IPublicationRecord> publicationRegistry, IMessageChannel channel)
		{
			this._publicationRegistry = publicationRegistry;
			this._channel = channel;
		}

		public Guid PublishMessage(IMessage message)
		{
			if (message.Identifier == Guid.Empty)
			{
				message.Identifier = Guid.NewGuid();
			}

			if (this._channel.State != ChannelState.Open)
			{
				this.WriteDebugMessage("Publication to closed channel detected. Attempting to open channel.");

				this._channel.Open();

				this.WriteDebugMessage("Channel has been opened, publication may proceed.");
			}

			var record = this._publicationRegistry.PublishMessage(message);

			this._channel.Send(record);

			this.WriteInfoMessage(
				string.Format(
					"Message {0} (record {1}) was successfully published via the channel {2}({3}).", 
					message.GetType().Name, 
					record.Identifier, 
					this._channel.GetType().Name, 
					this._channel.ChannelName));

			return record.Identifier;
		}
	}
}