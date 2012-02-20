using Euclid.Framework.Cqrs;

namespace Euclid.Sdk.TestAgent.Commands
{
	public class ComplexCommand : DefaultCommand
	{
		public int StringLength { get; set; }

		public string StringValue { get; set; }
	}

	public class UnsupportedCommand : DefaultCommand
	{
		public string SomeDate { get; set; }
	}
}