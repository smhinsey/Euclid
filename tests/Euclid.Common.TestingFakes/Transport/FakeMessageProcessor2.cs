using Euclid.Common.Messaging;

namespace Euclid.Common.TestingFakes.Transport
{
	public class FakeMessageProcessor2 : DefaultMessageProcessor<FakeMessage>
	{
		public static bool ProcessedAnyMessages;

		public override void Process(FakeMessage message)
		{
			ProcessedAnyMessages = true;
		}
	}
}