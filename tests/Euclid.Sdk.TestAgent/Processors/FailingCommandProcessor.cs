using System;
using Euclid.Framework.Cqrs;
using Euclid.Sdk.TestAgent.Commands;

namespace Euclid.Sdk.TestAgent.Processors
{
	public class FailingCommandProcessor : DefaultCommandProcessor<FailingCommand>
	{
		public override void Process(FailingCommand message)
		{
			throw new NotImplementedException();
		}
	}
}
