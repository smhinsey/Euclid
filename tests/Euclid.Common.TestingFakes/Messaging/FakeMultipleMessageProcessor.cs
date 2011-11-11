using Euclid.Common.Messaging;
using Euclid.Common.TestingFakes.Transport;

namespace Euclid.Common.TestingFakes.Messaging
{
	public class FakeMultipleMessageProcessor : MultipleMessageProcessor
	{
		public static int ProcessedMessages;

		 public void Process(FakeMessage message)
		 {
		 	ProcessedMessages++;
		 }

		 public void Process(DifferentFakeMessage message)
		 {
			 ProcessedMessages++;
		 }
	}
}