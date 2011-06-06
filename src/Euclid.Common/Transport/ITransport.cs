using System.Collections.Generic;

namespace Euclid.Common.Transport
{
	public interface ITransport
	{
		TransportState State { get; }

		TransportState Close();
		TransportState Open();

		IEnvelope Receive();
		IEnumerable<IEnvelope> Receive(int num);
		IEnumerable<IEnvelope> ReceiveAll();
		void Send(IEnvelope message);
	}
}