using Euclid.Common.Transport;

namespace Euclid.Common.TestingFakes.Transport
{
	public class FakeMessageProcessor : DefaultMessageProcessor<FakeMessage>
	{
		public static int MessageCount;
		public static bool ProcessedAnyMessages;

		public override void Process(FakeMessage message)
		{
			ProcessedAnyMessages = true;
			++MessageCount;
		}
	}
}