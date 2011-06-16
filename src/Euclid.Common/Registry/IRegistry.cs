using System;
using Euclid.Common.Transport;

namespace Euclid.Common.Registry
{
    public interface IRegistry<out TRecord>
        where TRecord : class, IRecord
    {
        TRecord CreateRecord(IMessage message);
        TRecord GetRecord(Guid identifier);
        IMessage GetMessage(Uri messageLocation, Type messageType);

        TRecord MarkAsComplete(Guid id);
        TRecord MarkAsFailed(Guid id, string message = null, string callStack = null);
    }
}