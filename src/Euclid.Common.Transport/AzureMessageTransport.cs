using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.ServiceModel.Security;
using System.Text;
using Euclid.Common.Serialization;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Euclid.Common.Transport
{
    public class AzureMessageTransport : MessageTransportBase
    {
        private static CloudQueue _queue = null;
        private static readonly object Down = new object();

        public AzureMessageTransport() : base()
        {
           
        }

        public override TransportState Close()
        {
            lock(Down)
            {
                _queue.Delete();
                _queue = null;
                State = TransportState.Closed;
            }

            return State;
        }

        public override TransportState Open()
        {
            lock(Down)
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

            var list = new List<IMessage>();

            var messages = _queue.GetMessages(howMany);

            var count = messages.Count();

            foreach(var message in messages)
            {
                if (count < 1 && DateTime.Now.Subtract(start) <= timeout) break;

                if (message == null) continue;

                _queue.DeleteMessage(message);

                count--;

                yield return JsonConvert.DeserializeObject(message.AsString) as IMessage;
            }

            yield break;
        }

        public override IMessage ReceiveSingle(TimeSpan timeSpan)
        {
            TransportIsOpenFor("ReceiveSingle");

            return ReceiveMany(1, timeSpan).First();
        }

        public override void Send(IMessage message)
        {
            TransportIsOpenFor("Send");

            var msg = MessageIsNotTooBig(message);

            _queue.AddMessage(msg);
        }

        public override int Clear()
        {
            var numMessages = _queue.ApproximateMessageCount;

            _queue.Clear();

            return !numMessages.HasValue ? 0 : numMessages.Value;
        }

        public override void DeleteMessage(IMessage message)
        {
            TransportIsOpenFor("DeleteMessage");
            
            var msg = MessageIsNotTooBig(message);

            _queue.DeleteMessage(msg);
        }

        public override IMessage Peek()
        {
            TransportIsOpenFor("Peek");

            var msg = _queue.PeekMessage();

            //return JsonConvert.DeserializeObject(msg.AsString);

            return null;
        }

        private static CloudQueueMessage MessageIsNotTooBig(IMessage message)
        {
            var settings = new JsonSerializerSettings
                               {
                                   TypeNameHandling = TypeNameHandling.Objects,
                                   ContractResolver = new CamelCasePropertyNamesContractResolver()
                               };

            var msg = JsonConvert.SerializeObject(message, Formatting.None, settings);

            if ((msg.Length / 1024) > 8)
            {
                throw new MessageSecurityException("The message is larger than 8k and can't be saved to the azure transport");                
            }

            return new CloudQueueMessage(msg);
        }

        private static void ValidNumberOfMessagesRequested(int howMany)
        {
            if (howMany > 32)
            {
                throw new InvalidOperationException("Only 32 messages can be retrieved from an azure transport at a time");
            }
        }

        private static void NextVisibleTimeIsValid(TimeSpan timeSpan)
        {
            if (timeSpan > new TimeSpan(0, 2, 0, 0))
            {
                throw new InvalidOperationException("The max timespan for an azure transport is 2 hours");
            }
        }

        private static void CreateQueue(string transportName)
        {
            var storageAccount = CloudStorageAccount.FromConfigurationSetting("DataConnectionString");
            var queueClient = storageAccount.CreateCloudQueueClient();

            _queue = queueClient.GetQueueReference(transportName);
            _queue.CreateIfNotExist();
        }
    }
}
