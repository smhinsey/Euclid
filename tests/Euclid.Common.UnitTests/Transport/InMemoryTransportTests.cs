using Euclid.Common.Transport;
using NUnit.Framework;

namespace Euclid.Common.UnitTests.Transport
{
	[TestFixture]
	public class InMemoryTransportTests
	{
		private const int LargeNumber = 1000000;

		[Test]
		public void TestClear()
		{
			TestTransport.Clear(new InMemoryMessageTransport());
		}

		[Test]
		public void TestLargeNumberOfMessagesSynchronously()
		{
			TestTransport.TestScale(new InMemoryMessageTransport(), LargeNumber);
		}

		[Test]
		public void TestSendReceive()
		{
			TestTransport.SendAndReceive(new InMemoryMessageTransport());
		}

		[Test]
		public void TestStateTransitions()
		{
			TestTransport.StateTransitions(new InMemoryMessageTransport());
		}

		[Test]
		public void TestTimeout()
		{
			TestTransport.ReceiveTimeout(new InMemoryMessageTransport());
		}
	}
}