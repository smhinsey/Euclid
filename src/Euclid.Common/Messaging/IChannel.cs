using System;
using System.Collections.Generic;

namespace Euclid.Common.Messaging
{
	// SELF this should be combined with IMessageChannel, also this TSubType stuff is is super confusing
	public interface IChannel<T>
	{
		ChannelState State { get; }
		string TransportName { get; set; }
		void Clear();
		ChannelState Close();
		ChannelState Open();

		IEnumerable<T> ReceiveMany(int howMany, TimeSpan timeSpan);
		IEnumerable<TSubType> ReceiveMany<TSubType>(int howMany, TimeSpan timeSpan) where TSubType : T;
		T ReceiveSingle(TimeSpan timeSpan);

		void Send(T message);
	}
}