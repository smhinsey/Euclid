using System.Data;
using Euclid.Framework.Cqrs;

namespace Euclid.Sdk.TestAgent.Commands
{
	public class FailingCommand : DefaultCommand
	{
		public int Number { get; set; }
	}
}