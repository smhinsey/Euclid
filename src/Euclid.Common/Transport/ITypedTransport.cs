using System.Collections.Generic;

namespace Euclid.Common.Transport
{
    public interface ITypedTransport<T>
    {
        TransportState State { get; }
        TransportState Open();
        TransportState Close();

        void Send(T message);
        IEnumerable<T> Receive(int num);
    }
}