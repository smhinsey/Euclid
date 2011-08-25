using System;
using Euclid.Common.Messaging;
using Euclid.TestingSupport;
using NUnit.Framework;

namespace Euclid.Common.UnitTests.Transport
{
	[TestFixture]
	[Category(TestCategories.Unit)]
	public class InMemoryTransportTests
	{
		private const int LargeNumber = 1000000;

		[Test]
		public void TestClear()
		{
			TestTransport.Clear(new InMemoryMessageChannel());
		}

		[Test]
		public void TestSendReceive()
		{
			TestTransport.SendAndReceiveSingleMessage(new InMemoryMessageChannel());
		}

		[Test]
		public void TestSendingMessageOnClosedTransport()
		{
			TestTransport.TestSendingMessageOnClosedTransport(new InMemoryMessageChannel());
		}

		[Test]
		public void TestStateTransitions()
		{
			TestTransport.StateTransitions(new InMemoryMessageChannel());
		}

		[Test]
		public void TestThroughputAsynchronously()
		{
			TestTransport.TestThroughputAsynchronously(new InMemoryMessageChannel(), LargeNumber, 17);

			Console.WriteLine();

			TestTransport.TestThroughputAsynchronously(new InMemoryMessageChannel(), LargeNumber, 17, 32);
		}

		[Test]
		public void TestThroughputSynchronously()
		{
			TestTransport.TestThroughputSynchronously(new InMemoryMessageChannel(), LargeNumber, null);
		}

		[Test]
		public void TestTimeout()
		{
			TestTransport.ReceiveTimeout(new InMemoryMessageChannel());
		}
	}
}
