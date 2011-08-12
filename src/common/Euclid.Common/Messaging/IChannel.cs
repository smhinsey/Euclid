using System;
using System.Collections.Generic;

namespace Euclid.Common.Messaging
{
	public interface IChannel<T>
	{
		string ChannelName { get; set; }
		ChannelState State { get; }
		void Clear();
		ChannelState Close();
		ChannelState Open();

		IEnumerable<T> ReceiveMany(int howMany, TimeSpan timeSpan);
		T ReceiveSingle(TimeSpan timeSpan);

		void Send(T message);
	}
}