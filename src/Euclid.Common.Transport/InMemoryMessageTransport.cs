using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Euclid.Common.Transport
{
	public class InMemoryMessageTransport : IMessageTransport
	{
        private static readonly ConcurrentQueue<IEnvelope> Queue = new ConcurrentQueue<IEnvelope>();

		public InMemoryMessageTransport()
		{
			State = TransportState.Invalid;
		}

		public TransportState State { get; private set; }

		public TransportState Close()
		{
			State = TransportState.Closed;

			return State;
		}

		public TransportState Open()
		{
			State = TransportState.Open;

			return State;
		}

		public IEnumerable<IEnvelope> ReceiveMany(int howMany, TimeSpan timeout)
		{
			TransportIsOpen("Send");

			var start = DateTime.Now;

			var count = 0;

			while (Queue.Count > 0 && DateTime.Now.Subtract(start) <= timeout && count < howMany)
			{
				IEnvelope message;

				Queue.TryDequeue(out message);

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

		public void Send(IEnvelope message)
		{
			TransportIsOpen("Send");

			if (message.Identifier == Guid.Empty)
			{
				message.Identifier = Guid.NewGuid();
			}

			Queue.Enqueue(message);
		}

	    public int Clear()
	    {
	        TransportIsOpen("Clear");

	        var count = 0;
            while(!Queue.IsEmpty)
            {
                IEnvelope m = null;
                if ( !Queue.TryDequeue(out m))
                {
                    throw new ApplicationException("Unable to clear InMemoryTransport");          
                }

                count++;
            }

	        return count;
	    }

	    public IEnvelope Peek()
	    {
	        IEnvelope message;
	        Queue.TryPeek(out message);

	        return message;
	    }

	    public void Delete(IEnvelope message)
	    {
            throw new NotImplementedException("You cannot delete messages from an InMemoryTransport");
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