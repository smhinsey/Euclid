using System;
using System.Collections.Generic;

namespace Euclid.Common.Transport
{
	public interface ITypeTransport<T>
	{
        string TransportName { get; set; }

        TransportState State { get; }
		TransportState Close();
		TransportState Open();

		IEnumerable<T> ReceiveMany(int howMany, TimeSpan timeSpan);
        T ReceiveSingle(TimeSpan timeSpan);

		void Send(T message);
	    int Clear();

        void DeleteMessage(T type);
	    T Peek();
	}
}