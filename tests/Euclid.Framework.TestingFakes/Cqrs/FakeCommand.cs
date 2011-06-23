using System;
using Euclid.Common.Messaging;
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

    public class FakeCommandProcessor : ICommandProcessor<FakeCommand>, ICommandProcessor<FakeCommand2>
    {
        public static int FakeCommandCount;
        public static int FakeCommandTwoCount;
        public void Process(FakeCommand command)
        {
            FakeCommandCount++;
        }

        public void Process(FakeCommand2 message)
        {
            FakeCommandTwoCount++;
        }
    }
}
