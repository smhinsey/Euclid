using System;
using Euclid.Common.Registry;
using Euclid.Common.Transport;

namespace Euclid.Common.TestingFakes.Registry
{
    public class FakeRecord
    {
        public Guid Identifier { get { return Message == null ? Guid.Empty : Message.Identifier; }}
        public Guid CreatedBy { get; set; }
        public DateTime Created { get; set; }
        public string CallStack { get; set; }
        public bool Dispatched { get; set; }
        public bool Error { get; set; }
        public string ErrorMessage { get; set; }
        public IMessage Message { get; set; }
    }
}