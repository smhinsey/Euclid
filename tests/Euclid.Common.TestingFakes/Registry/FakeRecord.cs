using System;
using Euclid.Common.Registry;
using Euclid.Common.Transport;

namespace Euclid.Common.TestingFakes.Registry
{
    public class FakeRecord : IRecord<FakeMessage>
    {
        public Guid Identifier { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime Created { get; set; }
        public string CallStack { get; set; }
        public bool Dispatched { get; set; }
        public bool Error { get; set; }
        public string ErrorMessage { get; set; }

        public FakeMessage Message { get; set; }
    }
}