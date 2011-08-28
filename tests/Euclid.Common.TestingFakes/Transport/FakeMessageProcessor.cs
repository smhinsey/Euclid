using Euclid.Common.Messaging;

namespace Euclid.Common.TestingFakes.Transport
{
	public class FakeMessageProcessor : MessageProcessorBase<FakeMessage>
	{
		public static bool ProcessedAnyMessages;

		public override void Process(FakeMessage message)
		{
			ProcessedAnyMessages = true;
		}
	}
}