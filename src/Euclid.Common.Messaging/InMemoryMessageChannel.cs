using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Euclid.Common.Messaging
{
    public class InMemoryMessageChannel : MessageChannelBase
    {
        private static readonly ConcurrentQueue<IMessage> Queue = new ConcurrentQueue<IMessage>();

        public override void Clear()
        {
            TransportIsOpenFor("Clear");


            while (!Queue.IsEmpty)
            {
                IMessage m = null;
                if (!Queue.TryDequeue(out m))
                {
                    throw new ApplicationException("Unable to clear InMemoryTransport");
                }
            }
        }

        public override ChannelState Close()
        {
            State = ChannelState.Closed;

            return State;
        }

        public override ChannelState Open()
        {
            State = ChannelState.Open;

            return State;
        }

        public override IEnumerable<IMessage> ReceiveMany(int howMany, TimeSpan timeout)
        {
            TransportIsOpenFor("ReceiveMany");

            var start = DateTime.Now;

            var count = 0;

            while (Queue.Count > 0 && DateTime.Now.Subtract(start) <= timeout && count < howMany)
            {
                IMessage message;

                Queue.TryDequeue(out message);

                if (message == null) continue;

                count++;

                yield return message;
            }

            yield break;
        }

        public override IMessage ReceiveSingle(TimeSpan timeout)
        {
            TransportIsOpenFor("ReceiveSingle");

            return Queue.Count == 0 ? null : ReceiveMany(1, timeout).First();
        }

        public override void Send(IMessage message)
        {
            TransportIsOpenFor("Send");

            if (message.Identifier == Guid.Empty)
            {
                message.Identifier = Guid.NewGuid();
            }

            Queue.Enqueue(message);
        }
    }
}