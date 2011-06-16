using System;
using Euclid.Common.Transport;

namespace Euclid.Common.Registry
{
    public interface IRegistry<TRecord>
        where TRecord : class, IRecord, new()
    {
        TRecord CreateRecord(IMessage message);
        TRecord Get(Guid id);
        IMessage GetMessage(TRecord record);

        TRecord MarkAsComplete(Guid id);
        TRecord MarkAsFailed(Guid id, string message = null, string callStack = null);
    }
}