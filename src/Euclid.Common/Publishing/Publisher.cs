using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Euclid.Common.Registry;
using Euclid.Common.Transport;

namespace Euclid.Common.Publishing
{
    public interface IPublisher
    {
        Guid PublishMessage(IMessage message);
    }
}
