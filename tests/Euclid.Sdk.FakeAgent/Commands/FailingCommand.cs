using Euclid.Framework.Cqrs;

namespace Euclid.Sdk.FakeAgent.Commands
{
	public class FailingCommand : DefaultCommand
	{
		public int Number { get; set; }
	}
}