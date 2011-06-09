using System;
using System.Collections.Generic;

namespace Euclid.Common.Transport
{
	public interface ITypeTransport<T>
	{
		TransportState State { get; }
		string TransportName { get; set; }
		void Clear();
		TransportState Close();
		TransportState Open();

		IEnumerable<T> ReceiveMany(int howMany, TimeSpan timeSpan);
		T ReceiveSingle(TimeSpan timeSpan);

		void Send(T message);
	}
}