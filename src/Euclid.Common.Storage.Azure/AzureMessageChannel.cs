using System;
using System.Collections.Generic;
using System.ServiceModel.Security;
using System.Text;
using Euclid.Common.Extensions;
using Euclid.Common.Messaging;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;

namespace Euclid.Common.Storage.Azure
{
	public class AzureMessageChannel : MessageChannelBase
	{
		private const int MaximumNumberOfMessagesThatCanBeFetched = 32;
		private static readonly object Down = new object();
		private static CloudQueue _queue;
		private readonly IMessageSerializer _serializer;

		public AzureMessageChannel(IMessageSerializer serializer)
		{
			_serializer = serializer;
		}

		private AzureMessageChannel()
		{
		}

		public override void Clear()
		{
			_queue.Clear();
		}

		public override ChannelState Close()
		{
			lock (Down)
			{
				_queue.Delete();
				_queue = null;
				State = ChannelState.Closed;
			}

			return State;
		}

		public override ChannelState Open()
		{
			lock (Down)
			{
				if (_queue == null)
				{
					CreateQueue(ChannelName);
					State = ChannelState.Open;
				}
			}

			return State;
		}

		public override IEnumerable<IMessage> ReceiveMany(int howMany, TimeSpan timeout)
		{
			TransportIsOpenFor("ReceiveMany");

			ValidNumberOfMessagesRequested(howMany);

			var start = DateTime.Now;

			var count = 0;

			while (count < howMany && DateTime.Now.Subtract(start) <= timeout)
			{
				var message = _queue.GetMessage();

				count++;

				if (message == null) continue;

				_queue.DeleteMessage(message);

				yield return _serializer.Deserialize(message.AsBytes);
			}

			yield break;
		}

		public override IMessage ReceiveSingle(TimeSpan timeSpan)
		{
			TransportIsOpenFor("ReceiveSingle");

			var msg = _queue.GetMessage();

			_queue.DeleteMessage(msg);

			return _serializer.Deserialize(msg.AsBytes);
		}

		public override void Send(IMessage message)
		{
			TransportIsOpenFor("Send");

			var msg = MessageIsNotTooBig(message);

			_queue.AddMessage(msg);
		}

		private CloudQueueMessage MessageIsNotTooBig(IMessage message)
		{
			var msgBytes = _serializer.Serialize(message);

			var msg = msgBytes.GetString(Encoding.UTF8);

			if ((msg.Length/1024) > 8)
			{
				throw new MessageSecurityException("The message is larger than 8k and can't be saved to the azure channel");
			}

			return new CloudQueueMessage(msg);
		}

		private static void CreateQueue(string channelName)
		{
			var storageAccount = CloudStorageAccount.FromConfigurationSetting("DataConnectionString");
			var queueClient = storageAccount.CreateCloudQueueClient();

			_queue = queueClient.GetQueueReference(channelName);
			_queue.CreateIfNotExist();
		}

		private static void ValidNumberOfMessagesRequested(int howMany)
		{
			if (howMany > MaximumNumberOfMessagesThatCanBeFetched)
			{
				throw new InvalidOperationException(string.Format("Only {0} messages can be retrieved from an azure channel at a time", MaximumNumberOfMessagesThatCanBeFetched));
			}
		}
	}
}