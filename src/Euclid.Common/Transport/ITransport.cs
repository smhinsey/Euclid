using System;
using System.Collections.Generic;

namespace Euclid.Common.Transport
{
	public interface ITransport
	{
		TransportState State { get; }
		TransportState Close();
		TransportState Open();

        void Send(IEnvelope message);
        IEnumerable<IEnvelope> ReceiveMany(int howMany, TimeSpan timeout);
        IEnvelope ReceiveSingle(TimeSpan timeout);
    }
}