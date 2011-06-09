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

				yield return _serializer.Deserialize(message.AsString.ToMemoryStream(Encoding.UTF8));
			}

			yield break;
		}

		public override IMessage ReceiveSingle(TimeSpan timeSpan)
		{
			TransportIsOpenFor("ReceiveSingle");

			var msg = _queue.GetMessage();

			_queue.DeleteMessage(msg);

			return _serializer.Deserialize(msg.AsString.ToMemoryStream(Encoding.UTF8));
		}

		public override void Send(IMessage message)
		{
			TransportIsOpenFor("Send");

			var msg = MessageIsNotTooBig(message);

			_queue.AddMessage(msg);
		}

		private CloudQueueMessage MessageIsNotTooBig(IMessage message)
		{
			var msgStream = _serializer.Serialize(message);

			var msg = msgStream.GetString(Encoding.UTF8);

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

		private static void NextVisibleTimeIsValid(TimeSpan timeSpan)
		{
			if (timeSpan > new TimeSpan(0, 2, 0, 0))
			{
				throw new InvalidOperationException("The max timespan for an azure transport is 2 hours");
			}
		}

		private static void ValidNumberOfMessagesRequested(int howMany)
		{
			if (howMany > 32)
			{
				throw new InvalidOperationException("Only 32 messages can be retrieved from an azure transport at a time");
			}
		}
	}
}