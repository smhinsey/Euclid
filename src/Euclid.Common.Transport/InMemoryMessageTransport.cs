using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Euclid.Common.Transport
{
	public class InMemoryMessageTransport : IMessageTransport
	{
		private readonly ConcurrentQueue<IEnvelope> _queue;

		public InMemoryMessageTransport()
		{
			State = TransportState.Invalid;
			_queue = new ConcurrentQueue<IEnvelope>();
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

		public void Send(IEnvelope message)
		{
			TransportIsOpen("Send");

			if (message.Identifier == Guid.Empty)
			{
				message.Identifier = Guid.NewGuid();
			}

			_queue.Enqueue(message);
		}

	    public int Clear()
	    {
	        TransportIsOpen("Clear");

	        var count = 0;
            while(!_queue.IsEmpty)
            {
                IEnvelope m = null;
                if ( !_queue.TryDequeue(out m))
                {
                    throw new ApplicationException("Unable to clear InMemoryTransport");          
                }

                count++;
            }

	        return count;
	    }

	    public int Delete(IEnvelope message)
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