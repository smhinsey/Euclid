using Euclid.Common.Transport;
using NUnit.Framework;

namespace Euclid.Common.UnitTests.Transport
{
    [TestFixture]
    public class InMemoryTransportTest
    {
        private ITransport _t = new InMemoryTransport();


        [Test]
        public void TestStateTransisitons()
        {
            TransportTests.TestTransportStateTransitions(_t);
        }

        [Test]
        public void TestSendReceive()
        {
            TransportTests.TestSendReceive(_t);
        }

        [Test]
        public void TestTimeout()
        {
            TransportTests.TestTransportTimeout(_t);
        }
    }
}