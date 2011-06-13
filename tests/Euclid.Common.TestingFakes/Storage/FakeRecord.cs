using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Euclid.Common.Registry;

namespace Euclid.Common.TestingFakes.Storage
{
    public class FakeRecord : IRecord<FakeMessage>
    {
        public DateTime Created { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid Identifier { get; set; }
        public string CallStack { get; set; }
        public bool Completed { get; set; }
        public bool Error { get; set; }
        public string ErrorMessage { get; set; }
        public FakeMessage Message { get; set; }
    }
}
