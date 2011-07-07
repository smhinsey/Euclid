﻿using System;
using System.Collections.Generic;

namespace Euclid.Common.Messaging
{
	// SELF this should be combined with IMessageChannel, also this TSubType stuff is is super confusing
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