using System;
using Euclid.Framework.Cqrs;
using Euclid.Sdk.FakeAgent.Commands;

namespace Euclid.Sdk.FakeAgent.Processors
{
	public class FailingCommandProcessor : DefaultCommandProcessor<FailingCommand>
	{
		public override void Process(FailingCommand message)
		{
			throw new NotImplementedException();
		}
	}
}