using System.Collections.Generic;

namespace Euclid.Framework.Cqrs
{
    public interface ITransport
    {
        TransportState State { get; }

        TransportState Open();
        TransportState Close();

        void Send(IEnvelope message);
        IEnvelope Receive();
        IEnumerable<IEnvelope> Receive(int num);
        IEnumerable<IEnvelope> ReceiveAll();
    }
}