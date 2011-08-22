using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using Euclid.Common.Extensions;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;

namespace Euclid.Common.Messaging.Azure
{
	public class AzureMessageChannel : MessageChannelBase
	{
		private const int MaximumNumberOfMessagesThatCanBeFetched = 32;

		private static readonly object Down = new object();

		private readonly IMessageSerializer _serializer;

		private static CloudQueue _queue;

		public AzureMessageChannel(IMessageSerializer serializer)
		{
			this._serializer = serializer;
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
				this.State = ChannelState.Closed;
			}

			return this.State;
		}

		public override ChannelState Open()
		{
			lock (Down)
			{
				if (_queue == null)
				{
					CreateQueue(this.ChannelName);
				}

				this.State = ChannelState.Open;
			}

			return this.State;
		}

		public override IEnumerable<IMessage> ReceiveMany(int howMany, TimeSpan timeout)
		{
			this.TransportIsOpenFor("ReceiveMany");

			ValidNumberOfMessagesRequested(howMany);

			var start = DateTime.Now;

			var count = 0;

			while (count < howMany && DateTime.Now.Subtract(start) <= timeout)
			{
				var message = _queue.GetMessage();

				count++;

				if (message == null)
				{
					continue;
				}

				_queue.DeleteMessage(message);

				yield return this._serializer.Deserialize(message.AsBytes);
			}

			yield break;
		}

		public override IMessage ReceiveSingle(TimeSpan timeSpan)
		{
			this.TransportIsOpenFor("ReceiveSingle");

			var msg = _queue.GetMessage();

			_queue.DeleteMessage(msg);

			return this._serializer.Deserialize(msg.AsBytes);
		}

		public override void Send(IMessage message)
		{
			this.TransportIsOpenFor("Send");

			var msg = this.MessageIsNotTooBig(message);

			_queue.AddMessage(msg);
		}

		private static void CreateQueue(string channelName)
		{
			CloudStorageAccount.SetConfigurationSettingPublisher(
				(configurationKey, publishConfigurationValue) =>
					{
						var connectionString = RoleEnvironment.IsAvailable
						                       	? RoleEnvironment.GetConfigurationSettingValue(configurationKey)
						                       	: ConfigurationManager.AppSettings[configurationKey];

						publishConfigurationValue(connectionString);
					});

			var storageAccount = CloudStorageAccount.FromConfigurationSetting("DataConnectionString");
			var queueClient = storageAccount.CreateCloudQueueClient();

			_queue = queueClient.GetQueueReference(channelName);
			_queue.CreateIfNotExist();
		}

		private static void ValidNumberOfMessagesRequested(int howMany)
		{
			if (howMany > MaximumNumberOfMessagesThatCanBeFetched)
			{
				throw new InvalidOperationException(
					string.Format(
						"Only {0} messages can be retrieved from an azure channel at a time", MaximumNumberOfMessagesThatCanBeFetched));
			}
		}

		private CloudQueueMessage MessageIsNotTooBig(IMessage message)
		{
			var msgBytes = this._serializer.Serialize(message);

			var msg = msgBytes.GetString(Encoding.UTF8);

			if ((msg.Length / 1024) > 8)
			{
				throw new Exception("The message is larger than 8k and can't be saved to the azure channel");
			}

			return new CloudQueueMessage(msg);
		}
	}
}