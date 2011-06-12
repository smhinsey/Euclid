using System;
using Euclid.Common.Registry;
using Euclid.Common.Transport;

namespace Euclid.Common.Storage
{
    public interface IBasicRecordRepository<TRecord, in TMessage>
        where TRecord : IRecord<TMessage>
        where TMessage : IMessage
    {
        TRecord Create(TMessage message);
        TRecord Retrieve(Guid id);
        TRecord Update(TRecord record);
        TRecord Delete(Guid id);
    }
}