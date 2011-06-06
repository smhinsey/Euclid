using System;
using System.IO;

namespace Euclid.Common.Transport
{
    public interface IEnvelope
    {
        Stream Message { get; set; }
        Type MessageType { get; set; }
        Guid Identifier { get; set; }
        bool Dispatched { get; set; }
        bool Error { get; set; }
        string CallStack { get; set; }
        string ErrorMessage { get; set; }
    }
}
