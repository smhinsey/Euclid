using System;
using Euclid.Common.Transport;

namespace Euclid.Common.TestingFakes.Registry
{
    public class FakeMessage : IMessage
    {
        public Guid Identifier { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime Created { get; set; }
    }
}