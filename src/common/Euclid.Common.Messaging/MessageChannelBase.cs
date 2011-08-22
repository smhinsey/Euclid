using System;
using System.Collections.Generic;

namespace Euclid.Common.Messaging
{
	public abstract class MessageChannelBase : IMessageChannel
	{
		// SELF why does ChannelName need to be a new guid in order for AzureMessageChannel to work?
		protected MessageChannelBase()
		{
			this.State = ChannelState.Invalid;
			this.ChannelName = Guid.NewGuid().ToString();
		}

		public string ChannelName { get; set; }

		public ChannelState State { get; protected set; }

		public abstract void Clear();

		public abstract ChannelState Close();

		public abstract ChannelState Open();

		public abstract IEnumerable<IMessage> ReceiveMany(int howMany, TimeSpan timeout);

		public abstract IMessage ReceiveSingle(TimeSpan timeout);

		public abstract void Send(IMessage message);

		protected void TransportIsOpenFor(string operationName)
		{
			if (this.State != ChannelState.Open)
			{
				throw new InvalidOperationException(
					string.Format("Cannot {0} a message when the channel is not open", operationName));
			}
		}
	}
}