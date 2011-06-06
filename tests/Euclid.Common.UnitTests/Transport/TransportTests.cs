
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Euclid.Common.TestingFakes.Transport;
using Euclid.Common.Transport;
using NUnit.Framework;

namespace Euclid.Common.UnitTests.Transport
{
    public class TransportTests
    {
        public static void TestTransportStateTransitions(ITransport transport)
        {
            Assert.AreEqual(TransportState.Invalid, transport.State);

            var newState = transport.Open();
            Assert.AreEqual(TransportState.Open, newState);

            newState = transport.Close();
            Assert.AreEqual(TransportState.Closed, newState);
        }

        public static void TestSendReceive(ITransport transport)
        {
            var ids = new List<Guid>();

            transport.Open();

            for (var i = 0; i < 100; i++ )
            {
                var message = new FakeMessage();
                transport.Send(message);

                ids.Add(message.Identifier);
            }

            for (int i = 0; i < 10; i++ )
            {
                var j = 0;
                foreach(var message in transport.ReceiveMany(10, TimeSpan.MaxValue))
                {
                    Assert.True(ids.Contains(message.Identifier));
                    j++;
                }

                Assert.AreEqual(10, j);
            }
                
            transport.Close();
        }

        public static void TestTransportTimeout(ITransport transport)
        {
            var ts = new TimeSpan(0, 0, 0, 0, 100);

            transport.Open();

            var m = new FakeMessage();
            var m2 = new FakeMessage();

            transport.Send(m);
            transport.Send(m2);

            var count = 0;
            foreach(var msg in transport.ReceiveMany(2, ts))
            {
                count++;
                Thread.Sleep(500);
            }

            Assert.AreEqual(1, count);
        }
    }
}