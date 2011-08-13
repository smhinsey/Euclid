using Euclid.Common.Logging;
using Euclid.Framework.Cqrs;
using Euclid.Sdk.FakeAgent.Commands;

namespace Euclid.Sdk.FakeAgent.Processors
{
	public class FakeCommandProcessor : DefaultCommandProcessor<FakeCommand>, ILoggingSource
	{
		public override void Process(FakeCommand message)
		{
			this.WriteInfoMessage("Command no. {0} was processed by FakeCommandProcessor", message.Number);
		}
	}
}