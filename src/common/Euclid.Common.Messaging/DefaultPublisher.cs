﻿using System;

namespace Euclid.Common.Messaging
{
	public class DefaultPublisher : IPublisher
	{
		private readonly IMessageChannel _channel;
		private readonly IPublicationRegistry<IPublicationRecord> _publicationRegistry;

		public DefaultPublisher(IPublicationRegistry<IPublicationRecord> publicationRegistry, IMessageChannel channel)
		{
			_publicationRegistry = publicationRegistry;
			_channel = channel;
		}

		public Guid PublishMessage(IMessage message)
		{
			if (_channel.State != ChannelState.Open)
			{
				_channel.Open();
			}

			var record = _publicationRegistry.CreateRecord(message);
			_channel.Send(record);

			return record.Identifier;
		}
	}
}