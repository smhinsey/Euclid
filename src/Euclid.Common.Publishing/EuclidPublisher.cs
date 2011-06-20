using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Euclid.Common.Registry;
using Euclid.Common.Transport;

namespace Euclid.Common.Publishing
{
    public class EuclidPublisher : IPublisher
    {
        private readonly IRegistry<IRecord> _registry;
        private readonly IMessageTransport _transport;

        public EuclidPublisher(IRegistry<IRecord> registry, IMessageTransport transport)
        {
            _registry = registry;
            _transport = transport;
        }

        public Guid PublishMessage(IMessage message)
        {
            if (_transport.State != TransportState.Open)
            {
                _transport.Open();
            }

            var record = _registry.CreateRecord(message);
            _transport.Send(record);

            return record.Identifier;
        }
    }
}
