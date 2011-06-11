using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Euclid.Common.Transport;

namespace Euclid.Common.Registry
{
    public interface IRecord<T> : IMessage where T : IMessage
    {
        string CallStack { get; set; }
        bool Dispatched { get; set; }
        bool Error { get; set; }
        string ErrorMessage { get; set; }
        T Message { get; set; }
    }

    public interface IRegistry<T, in TMessage> where T : IRecord<TMessage> where TMessage : IMessage
    {
        void Add(T record);
        T Get(Guid id);
        T CreateRecord(TMessage message);
    }
}
