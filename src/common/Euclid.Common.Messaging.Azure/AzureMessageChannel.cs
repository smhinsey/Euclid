using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using Euclid.Common.Extensions;
using Euclid.Common.Logging;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;

namespace Euclid.Common.Messaging.Azure
{
	public class AzureMessageChannel : DefaultMessageChannel, ILoggingSource
	{
		private static readonly object Locker = new object();

		private readonly IMessageSerializer _serializer;

		private CloudQueue _queue;

		public AzureMessageChannel(IMessageSerializer serializer)
		{
			_serializer = serializer;
		}

		private AzureMessageChannel()
		{
		}

		public override void Clear()
		{
			TransportIsOpenFor("Clear");

			_queue.Clear();
		}

		public override ChannelState Close()
		{
			lock (Locker)
			{
				_queue = null;
				State = ChannelState.Closed;
			}

			return State;
		}

		public override ChannelState Open()
		{
			this.WriteDebugMessage("Opening channel {0}", ChannelName);

			lock (Locker)
			{
				if (_queue == null)
				{
					openOrCreateQueue(ChannelName);
				}

				State = ChannelState.Open;
			}

			this.WriteDebugMessage("Opened channel {0}", ChannelName);

			return State;
		}

		public override IEnumerable<IMessage> ReceiveMany(int howMany, TimeSpan timeout)
		{
			TransportIsOpenFor("ReceiveMany");

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

				this.WriteDebugMessage("Message received from Azure Queue.");

				_queue.DeleteMessage(message);

				this.WriteDebugMessage("Message deleted from Azure Queue.");

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

			if ((msg.Length / 1024) > 8)
			{
				throw new Exception("The message is larger than 8k and can't be saved to the azure channel");
			}

			return new CloudQueueMessage(msg);
		}

		private void openOrCreateQueue(string channelName)
		{
			this.WriteDebugMessage("Attempting to open queue for channel {0}", channelName);

			try
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

				_queue = queueClient.GetQueueReference(channelName.ToLower());

				if (!_queue.Exists())
				{
					this.WriteDebugMessage("Queue {0} does not exist. Attempting to create it.", channelName);

					_queue.Create();

					this.WriteDebugMessage("Created queue for channel {0}", channelName);
				}
			}
			catch (Exception e)
			{
				this.WriteErrorMessage("Failed to open or create queue for channel {0}", e, channelName);

				throw;
			}

			this.WriteDebugMessage("Opened queue for channel {0}", channelName);
		}
	}
}