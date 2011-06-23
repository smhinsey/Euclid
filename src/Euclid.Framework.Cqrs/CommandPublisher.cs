using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Euclid.Common.Messaging;

namespace Euclid.Framework.Cqrs
{
    public class CommandPublisher : IPublisher
    {
        private readonly ICommandRegistry _registry;
        private readonly IMessageChannel _channel;

        public CommandPublisher(ICommandRegistry registry, IMessageChannel channel)
        {
            _registry = registry;
            _channel = channel;
        }

        public Guid PublishMessage(IMessage message)
        {
            if (_channel.State != ChannelState.Open)
            {
                _channel.Open();
            }

            var record = _registry.CreateRecord(message);
            _channel.Send(record);

            return record.Identifier;
        }
    }
}
