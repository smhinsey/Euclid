using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Euclid.Common.TestingFakes.Transport;
using Euclid.Common.Transport;
using NUnit.Framework;

namespace Euclid.Common.UnitTests.Transport
{
    [TestFixture]
    public class InMemoryTransportTests
    {
        [Test]
        public void TestStateTransitions()
        {
            TestTransport.StateTransitions(new InMemoryMessageTransport());
        }

        [Test]
        public void TestSendReceive()
        {
            TestTransport.SendAndReceive(new InMemoryMessageTransport());
        }

        [Test]
        public void TestTimeout()
        {
            TestTransport.ReceiveTimeout(new InMemoryMessageTransport());
        }

        [Test]
        public void TestClear()
        {
            TestTransport.Clear(new InMemoryMessageTransport());
        }

        [Test]
        public void TestDelete()
        {
            var t = new InMemoryMessageTransport();
            t.Open();

            Assert.Throws(typeof (NotImplementedException), () => t.DeleteMessage(null));

            var m = new FakeMessage();
            t.Send(m);
            Assert.Throws(typeof(NotImplementedException), () => t.DeleteMessage(m));

            t.Close();
        }

        [Test]
        public void TestPeek()
        {
            TestTransport.Peek(new InMemoryMessageTransport());
        }
    }
}
