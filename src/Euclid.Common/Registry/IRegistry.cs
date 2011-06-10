using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Euclid.Common.Transport;

namespace Euclid.Common.Registry
{
    public interface IRecord : IMessage
    {
        string CallStack { get; set; }
        bool Dispatched { get; set; }
        bool Error { get; set; }
        string ErrorMessage { get; set; }
    }

    public interface IRegistry<T> where T : IRecord
    {
        void Add(T record);
        T Get(Guid id);
        // T CreateRecord<TMessage>() where TMessage : IMessage;
    }
}
