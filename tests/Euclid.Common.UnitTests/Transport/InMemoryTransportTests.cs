using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Euclid.Common.Transport;
using NUnit.Framework;

namespace Euclid.Common.UnitTests.Transport
{
    [TestFixture]
    public class InMemoryTransportTests
    {
        private readonly ITransport _t = new InMemoryTransport();

        [Test]
        public void TestStateTransitions()
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
