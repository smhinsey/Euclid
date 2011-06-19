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

		public TransportState State { get; protected set; }
		public string TransportName { get; set; }
		public abstract void Clear();
		public abstract TransportState Close();
		public abstract TransportState Open();
		public abstract IEnumerable<IMessage> ReceiveMany(int howMany, TimeSpan timeout);

	    public abstract IEnumerable<TSubType> ReceiveMany<TSubType>(int howMany, TimeSpan timeSpan)
	        where TSubType : IMessage;

	    public abstract IMessage ReceiveSingle(TimeSpan timeout);
		public abstract void Send(IMessage message);

		protected void TransportIsOpenFor(string operationName)
		{
			if (State != TransportState.Open)
			{
				throw new InvalidOperationException
					(string.Format
					 	(
					 	 "Cannot {0} a message when the transport is not open",
					 	 operationName));
			}
		}
	}
}