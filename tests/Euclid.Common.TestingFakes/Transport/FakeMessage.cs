using System;
using System.IO;
using Euclid.Common.Transport;

namespace Euclid.Common.TestingFakes.Transport
{
    public class FakeMessage : IEnvelope
    {
        public FakeMessage()
        {
            
            Identifier = Guid.NewGuid();
        }
        public Stream Message { get; set; }
        public Type MessageType { get; set; }
        public Guid Identifier { get; set; }
        public bool Dispatched { get; set; }
        public bool Error { get; set; }
        public string CallStack { get; set; }
        public string ErrorMessage { get; set; }
    }
}