using System;
using System.Collections.Generic;
using System.ServiceModel.Security;
using System.Text;
using Euclid.Common.Extensions;
using Euclid.Common.Serialization;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;

namespace Euclid.Common.Transport
{
	public class AzureMessageTransport : MessageTransportBase
	{
		private const int MaximumNumberOfMessagesThatCanBeFetched = 32;
		private static readonly object Down = new object();
		private static CloudQueue _queue;
		private readonly IMessageSerializer _serializer;
		public AzureMessageTransport(IMessageSerializer serializer)
		{
			_serializer = serializer;
		}

		private AzureMessageTransport()
		{
		}

		public override void Clear()
		{
			_queue.Clear();
		}

		public override TransportState Close()
		{
			lock (Down)
			{
				_queue.Delete();
				_queue = null;
				State = TransportState.Closed;
			}

			return State;
		}

		public override TransportState Open()
		{
			lock (Down)
			{
				if (_queue == null)
				{
					CreateQueue(TransportName);
					State = TransportState.Open;
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

		public override IEnumerable<TSubType> ReceiveMany<TSubType>(int howMany, TimeSpan timeSpan)
		{
			TransportIsOpenFor(string.Format("ReceiveMany<{0}>", typeof (TSubType).Name));

			ValidNumberOfMessagesRequested(howMany);

			var start = DateTime.Now;

			var count = 0;

			var loopCount = 0;

			//NOTE: this is tricky:
			// if the message retrieved after the call to _queue.GetMessage isn't the type we are looking for it won't be available to be dequeued for 30s
			// if it is the type we are looking for we delete it form the queue
			// a message may get added to the queue while it is being enumerated, is that a problem?
			while (count < howMany && DateTime.Now.Subtract(start) <= timeSpan && loopCount <= MaximumNumberOfMessagesThatCanBeFetched)
			{
				loopCount++;

				var cloudQueueMessage = _queue.GetMessage();

				if (cloudQueueMessage == null) continue;

                var euclidMessage = _serializer.Deserialize(cloudQueueMessage.AsBytes);

				if (!(euclidMessage.GetType().IsAssignableFrom(typeof (TSubType)) || (euclidMessage.GetType().GetInterface(typeof (TSubType).FullName) != null)))
				{
					continue;
				}

				count++;

				_queue.DeleteMessage(cloudQueueMessage);

				yield return (TSubType) euclidMessage;
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
				throw new MessageSecurityException("The message is larger than 8k and can't be saved to the azure transport");
			}

			return new CloudQueueMessage(msg);
		}

		private static void CreateQueue(string transportName)
		{
			var storageAccount = CloudStorageAccount.FromConfigurationSetting("DataConnectionString");
			var queueClient = storageAccount.CreateCloudQueueClient();

			_queue = queueClient.GetQueueReference(transportName);
			_queue.CreateIfNotExist();
		}

		private static void ValidNumberOfMessagesRequested(int howMany)
		{
			if (howMany > MaximumNumberOfMessagesThatCanBeFetched)
			{
				throw new InvalidOperationException(string.Format("Only {0} messages can be retrieved from an azure transport at a time", MaximumNumberOfMessagesThatCanBeFetched));
			}
		}
	}
}