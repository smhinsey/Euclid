using System;
using System.Collections.Generic;
using Euclid.Common.Registry;
using Euclid.Common.Transport;
using NHibernate;

namespace Euclid.Common.Storage.Nhibernate
{
    public class NhibernateRecordRepository<TRecord, TMessage> : IBasicRecordRepository<TRecord, TMessage>
        where TRecord : IRecord<TMessage>, new()
        where TMessage : IMessage
    {
        protected ISession Session { get; set; }

        public TRecord Create(TMessage message)
        {
            var record = new TRecord
                             {
                                 Identifier = Guid.NewGuid(),
                                 Created = DateTime.Now,
                                 Message = message
                             };

            Session.Save(record);

            return record;
        }

        public TRecord Delete(Guid id)
        {
            var record = Retrieve(id);

            if (record == null)
            {
                throw new KeyNotFoundException();
            }

            Session.Delete(id);

            return record;
        }

        public TRecord Retrieve(Guid id)
        {
            return Session.Get<TRecord>(id);
        }

        public TRecord Update(TRecord record)
        {
            Session.Update(record, record.Identifier);

            return Retrieve(record.Identifier);
        }
    }
}
