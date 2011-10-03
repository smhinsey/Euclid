using Euclid.Framework.Cqrs;

namespace Euclid.Sdk.TestAgent.Commands
{
	public class ComplexCommand : DefaultCommand
	{
		public string StringValue { get; set; }
		public int StringLength { get; set; }
	}
}