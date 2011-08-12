using System;
using Euclid.Framework.Cqrs;
using Euclid.Sdk.FakeAgent.Commands;

namespace Euclid.Sdk.FakeAgent.Processors
{
	public class FakeCommandProcessor : DefaultCommandProcessor<FakeCommand>
	{
		public override void Process(FakeCommand message)
		{
			throw new NotImplementedException();
		}
	}
}