
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
        public static void StateTransitions(IMessageTransport transport)
        {
            Assert.AreNotEqual(TransportState.Closed, transport.State);

            var newState = transport.Open();
            transport.Clear();

            Assert.AreEqual(TransportState.Open, newState);

            newState = transport.Close();
            Assert.AreEqual(TransportState.Closed, newState);
        }

        public static void SendAndReceive(IMessageTransport transport)
        {
            var ids = new List<Guid>();

            transport.Open();
            transport.Clear();

            for (var i = 0; i < 100; i++ )
            {
                IMessage message = new FakeMessage();
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

        public static void ReceiveTimeout(IMessageTransport transport)
        {
            var ts = new TimeSpan(0, 0, 0, 0, 100);

            transport.Open();
            transport.Clear();

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

        public static void Clear(IMessageTransport transport)
        {
            transport.Open();
            transport.Clear();

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
            transport.Clear();

            IMessage toDelete = null;
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

            transport.DeleteMessage(toDelete);
            
            transport.Close();
        }

        public static void Peek(IMessageTransport transport)
        {
            transport.Open();
            transport.Clear();

            var m = new FakeMessage
                        {
                            Identifier = new Guid("AA2E58C4-84B2-4DE6-B3CC-FB8630959C0D")
                        };

            var m2 = new FakeMessage
                         {
                             Identifier = new Guid("940CE613-D375-4135-AAB9-23FA2173BD79")
                         };

            transport.Send(m);
            transport.Send(m2);

            var m3 = transport.Peek();
            Assert.AreEqual(m.Identifier, m3.Identifier);

            transport.Close();
        }
    }
}