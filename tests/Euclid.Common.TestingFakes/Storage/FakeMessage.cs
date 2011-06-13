using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Euclid.Common.Transport;

namespace Euclid.Common.TestingFakes.Storage
{
    public class FakeMessage : IMessage
    {
        public virtual DateTime Created { get; set; }
        public virtual Guid CreatedBy { get; set; }
        public virtual Guid Identifier { get; set; }
    }
}
