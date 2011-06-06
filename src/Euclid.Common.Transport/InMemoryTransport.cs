using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Euclid.Common.Transport
{
    public class InMemoryTransport : ITransport
    {
        public TransportState State { get; private set; }

        private readonly ConcurrentQueue<IEnvelope> _queue;

        public InMemoryTransport()
        {
            State = TransportState.Invalid;
            _queue = new ConcurrentQueue<IEnvelope>();
        }

        public TransportState Open()
        {
            State = TransportState.Open;

            return State;
        }

        public TransportState Close()
        {
            State = TransportState.Closed;

            return State;
        }

        public void Send(IEnvelope message)
        {
            TransportIsOpen("Send");

            if (message.Identifier == Guid.Empty)
            {
                message.Identifier = Guid.NewGuid();
            }

            _queue.Enqueue(message);
        }

        public IEnumerable<IEnvelope> ReceiveMany(int howMany, TimeSpan timeout)
        {
            TransportIsOpen("Send");

            var start = DateTime.Now;

            var count = 0;

            while (_queue.Count > 0 && DateTime.Now.Subtract(start) <= timeout && count < howMany)
            {
                IEnvelope message;

                _queue.TryDequeue(out message);

                if (message == null) continue;

                count++;

                yield return message;
            }

            yield break;
        }

        public IEnvelope ReceiveSingle(TimeSpan timeout)
        {
            return ReceiveMany(1, timeout).First();
        }

        public void TransportIsOpen(string operationName)
        {
            if (State != TransportState.Open)
            {
                throw new InvalidOperationException(string.Format(
                    "Cannot {0} a message when the transport is not open", operationName));
            }
            
        }
    }
}
