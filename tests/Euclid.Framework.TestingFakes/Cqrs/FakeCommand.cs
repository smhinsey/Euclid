using System;
using Euclid.Common.Messaging;
using Euclid.Framework.Cqrs;

namespace Euclid.Framework.TestingFakes.Cqrs
{
	public class FakeCommand : ICommand
	{
		public string CommandName { get; set; }
		public DateTime Created { get; set; }
		public Guid CreatedBy { get; set; }
		public Guid Identifier { get; set; }
	}

	public class FakeCommand2 : ICommand
	{
		public string CommandName { get; set; }
		public DateTime Created { get; set; }
		public Guid CreatedBy { get; set; }
		public Guid Identifier { get; set; }
	}


	public class FakeCommand3 : ICommand
	{
		public DateTime Created { get; set; }
		public Guid CreatedBy { get; set; }
		public Guid Identifier { get; set; }
	}

	public class FakeCommandProcessor : ICommandProcessor<FakeCommand>, ICommandProcessor<FakeCommand2>
	{
		public static int FakeCommandCount;
		public static int FakeCommandTwoCount;

		public bool CanProcessMessage(IMessage message)
		{
			return (message.GetType() == typeof (FakeCommand) || message.GetType() == typeof (FakeCommand2));
		}

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