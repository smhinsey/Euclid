
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Euclid.Common.TestingFakes.Transport;
using Euclid.Common.Transport;
using NUnit.Framework;

namespace Euclid.Common.UnitTests.Transport
{
    public class TestTransport
    {
        public static void StateTransitions(IMessageTransport messageTransport)
        {
            Assert.AreNotEqual(TransportState.Closed, messageTransport.State);

            var newState = messageTransport.Open();
            Assert.AreEqual(TransportState.Open, newState);

            newState = messageTransport.Close();
            Assert.AreEqual(TransportState.Closed, newState);
        }

        public static void SendAndReceive(IMessageTransport messageTransport)
        {
            var ids = new List<Guid>();

            messageTransport.Open();

            for (var i = 0; i < 100; i++ )
            {
                var message = new FakeMessage();
                messageTransport.Send(message);

                ids.Add(message.Identifier);
            }

            for (int i = 0; i < 10; i++ )
            {
                var j = 0;
                foreach(var message in messageTransport.ReceiveMany(10, TimeSpan.MaxValue))
                {
                    Assert.True(ids.Contains(message.Identifier));
                    j++;
                }

                Assert.AreEqual(10, j);
            }
                
            messageTransport.Close();
        }

        public static void ReceiveTimeout(IMessageTransport messageTransport)
        {
            var ts = new TimeSpan(0, 0, 0, 0, 100);

            messageTransport.Open();

            var m = new FakeMessage();
            var m2 = new FakeMessage();

            messageTransport.Send(m);
            messageTransport.Send(m2);

            var count = 0;
            foreach(var msg in messageTransport.ReceiveMany(2, ts))
            {
                count++;
                Thread.Sleep(500);
            }

            Assert.AreEqual(1, count);
        }

        public static void Clear(IMessageTransport transport)
        {
            transport.Open();

            for (var i = 0;i < 5;i++)
            {
                var m = new FakeMessage();
                transport.Send(m);
            }

            var numCleared = transport.Clear();
            Assert.AreEqual(5, numCleared);

            transport.Close();
        }

        public static void Delete(IMessageTransport transport)
        {
            transport.Open();

            IEnvelope toDelete = null;
            var r = new Random((int) DateTime.Now.Ticks);
            for (var i = 0; i < 5; i++)
            {
                var m = new FakeMessage();
                
                if (r.Next() % 4 == 0 && toDelete == null)
                {
                    toDelete = m;
                }
                transport.Send(m);
            }

            transport.Delete(toDelete);
            
            transport.Close();
        }
    }
}