using System;
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
        public void TestSendReceive()
        {
            TestTransport.SendAndReceiveSingleMessage(new InMemoryMessageTransport());
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

        [Test]
        public void TestThroughputSynchronously()
        {
            TestTransport.TestThroughputSynchronously(new InMemoryMessageTransport(), LargeNumber, null);
        }

        [Test]
        public void TestThroughputAsynchronously()
        {
            TestTransport.TestThroughputAsynchronously(new InMemoryMessageTransport(), LargeNumber, 17);

            Console.WriteLine();

            TestTransport.TestThroughputAsynchronously(new InMemoryMessageTransport(), LargeNumber, 17, 32);
        }

        [Test]
        public void TestSendingMessageOnClosedTransport()
        {
            TestTransport.TestSendingMessageOnClosedTransport(new InMemoryMessageTransport());
        }

    }
}
