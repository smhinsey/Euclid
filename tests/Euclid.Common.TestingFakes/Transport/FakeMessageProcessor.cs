﻿using Euclid.Common.Transport;

namespace Euclid.Common.TestingFakes.Transport
{
	public class FakeMessageProcessor : DefaultMessageProcessor<FakeMessage>
	{
		public static bool ProcessedAnyMessages = false;

		public override void Process(FakeMessage message)
		{
			ProcessedAnyMessages = true;
		}
	}
}