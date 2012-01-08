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
		private const int MaximumNumberOfMessagesThatCanBeFetched = 32;

		private static readonly object Down = new object();

		private readonly IMessageSerializer _serializer;

		private static CloudQueue _queue;

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
			this.WriteDebugMessage("Opening channel {0}", ChannelName);

			lock (Down)
			{
				if (_queue == null)
				{
					createQueue(ChannelName);
				}

				State = ChannelState.Open;
			}

			this.WriteDebugMessage("Opened channel {0}", ChannelName);

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
			var msgBytes = _serializer.Serialize(message);

			var msg = msgBytes.GetString(Encoding.UTF8);

			if ((msg.Length / 1024) > 8)
			{
				throw new Exception("The message is larger than 8k and can't be saved to the azure channel");
			}

			return new CloudQueueMessage(msg);
		}

		private void createQueue(string channelName)
		{
			this.WriteDebugMessage("Creating queue for channel {0}", channelName);

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

				_queue.CreateIfNotExist();
			}
			catch (Exception e)
			{
				this.WriteErrorMessage("Failed to create queue for channel {0}", e, channelName);
			}

			this.WriteDebugMessage("Created queue for channel {0}", channelName);
		}
	}
}