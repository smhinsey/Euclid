using System;
using System.Collections.Generic;

namespace Euclid.Common.Transport
{
    public abstract class MessageTransportBase : IMessageTransport
    {
        protected MessageTransportBase()
        {
            State = TransportState.Invalid;
            TransportName = Guid.NewGuid().ToString();
        }

        public string TransportName { get; set; }
        public TransportState State { get; protected set; }
        public abstract TransportState Close();
        public abstract TransportState Open();
        public abstract IEnumerable<IMessage> ReceiveMany(int howMany, TimeSpan timeout);
        public abstract IMessage ReceiveSingle(TimeSpan timeout);
        public abstract void Send(IMessage message);
        public abstract int Clear();
        public abstract IMessage Peek();
        public abstract void DeleteMessage(IMessage message);

        protected void TransportIsOpenFor(string operationName)
        {
            if (State != TransportState.Open)
            {
                throw new InvalidOperationException(string.Format(
                    "Cannot {0} a message when the transport is not open", operationName));
            }
        }
    }
}