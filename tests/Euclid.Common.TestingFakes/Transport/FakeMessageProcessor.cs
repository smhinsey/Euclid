using System;
using Euclid.Common.Transport;

namespace Euclid.Common.TestingFakes.Transport
{
	public class FakeMessageProcessor : DefaultMessageProcessor<FakeMessage>
	{
		public static bool ProcessedAnyMessages;
	    public static int MessageCount = 0;

		public override void Process(FakeMessage message)
		{
			ProcessedAnyMessages = true;
		    ++MessageCount;
		}
	}
}