﻿using System;
using System.Collections.Generic;

namespace Euclid.Common.Messaging
{
	public abstract class MessageChannelBase : IMessageChannel
	{
		protected MessageChannelBase()
		{
			State = ChannelState.Invalid;
			ChannelName = Guid.NewGuid().ToString();
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
			if (State != ChannelState.Open)
			{
				throw new InvalidOperationException
					(string.Format
					 	(
					 	 "Cannot {0} a message when the channel is not open",
					 	 operationName));
			}
		}
	}
}