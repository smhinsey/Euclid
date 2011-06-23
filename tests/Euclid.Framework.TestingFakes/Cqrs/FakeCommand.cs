using System;
using Euclid.Framework.Cqrs;

namespace Euclid.Framework.TestingFakes.Cqrs
{
    public class FakeCommand : ICommand
    {
        public DateTime Created { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid Identifier { get; set; }
        public string CommandName { get; set; }
    }

    public class FakeCommand2 : ICommand
    {
        public DateTime Created { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid Identifier { get; set; }
        public string CommandName { get; set; }
    }
}
