using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Euclid.Common.Registry;
using Euclid.Common.TestingFakes.Registry;
using Euclid.Common.Transport;
using NUnit.Framework;

namespace Euclid.Common.IntegrationTests
{
    [TestFixture]
    public class PublicationTests
    {
        private IRegistry<FakeRecord> _registry;
        private IMessageTransport _transport;

        public PublicationTests()
        {
            _registry = new FakeRegistry();
            _transport = new InMemoryMessageTransport();
        }

        [Test]
        public void TestSendMessageOverTransport()
        {
            _transport.Open();

            var msg = _registry.CreateRecord(new FakeMessage());

            _transport.Send(msg);

            var receivedMsg = _transport.ReceiveSingle(TimeSpan.MaxValue);

            Assert.NotNull(receivedMsg);

            Assert.NotNull(receivedMsg as IRecord);

            Assert.NotNull(receivedMsg as FakeRecord);

            _registry.Add(receivedMsg as FakeRecord);

            var d = _registry.Get(receivedMsg.Identifier);

            Assert.NotNull(d);

            _transport.Close();
        }
    }
}
