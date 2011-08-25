using Euclid.Framework.Cqrs;

namespace Euclid.Sdk.TestAgent.Commands
{
	public class TestCommand : DefaultCommand
	{
		public int Number { get; set; }
	}
}
