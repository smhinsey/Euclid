using System.Collections.Generic;

namespace Euclid.Common.Transport
{
	public interface ITypedTransport<T>
	{
		TransportState State { get; }
		TransportState Close();
		TransportState Open();

		IEnumerable<T> Receive(int num);
		void Send(T message);
	}
}