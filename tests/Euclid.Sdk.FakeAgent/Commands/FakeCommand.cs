using Euclid.Framework.Cqrs;

namespace Euclid.Sdk.FakeAgent.Commands
{
	public class FakeCommand : DefaultCommand
	{
		public int Number { get; set; }
	}
}