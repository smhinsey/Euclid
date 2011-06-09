using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Euclid.Common.TestingFakes.Transport;
using Euclid.Common.Transport;
using NUnit.Framework;

namespace Euclid.Common.UnitTests.Transport
{
	public class TestTransport
	{
		private static readonly Random Random = new Random((int) DateTime.Now.Ticks);

		public static void Clear(IMessageTransport transport)
		{
			transport.Open();

        public static void SendAndReceiveSingleMessage(IMessageTransport transport)
			{
				var m = new FakeMessage();
				transport.Send(m);
			}

			transport.Clear();

            var m = GetNewMessage();

            transport.Send(m);

			transport.Close();

            var m2 = transport.ReceiveSingle(TimeSpan.MaxValue);
			       		Error = Random.Next()%2 == 0,
            Assert.NotNull(m2);

            Assert.AreEqual(m.Identifier, m2.Identifier);
			       		         		Random.Next().ToString(),
			       		         		Random.Next().ToString(),
			       		         		Random.Next().ToString()
			       	};
		}

		public static void ReceiveTimeout(IMessageTransport transport)
		{
			var ts = new TimeSpan(0, 0, 0, 0, 500);

			transport.Open();
			transport.Clear();

			var m = new FakeMessage();
			var m2 = new FakeMessage();

			transport.Send(m);
			transport.Send(m2);

			var count = 0;
			foreach (var msg in transport.ReceiveMany(2, ts))
			{
				count++;
				Thread.Sleep(500);
			}

			Assert.AreEqual(1, count);
		}

		public static void SendAndReceive(IMessageTransport transport)
		{
			var ids = new List<Guid>();

			transport.Open();
			transport.Clear();

			for (var i = 0; i < 100; i++)
			{
				IMessage message = new FakeMessage();
				transport.Send(message);

				ids.Add(message.Identifier);
			}

			for (var i = 0; i < 10; i++)
			{
				var j = 0;
				foreach (var message in transport.ReceiveMany(10, TimeSpan.MaxValue))
				{
					Assert.True(ids.Contains(message.Identifier));
					j++;
				}

				Assert.AreEqual(10, j);
			}

			transport.Close();
		}

		public static void StateTransitions(IMessageTransport transport)
		{
			Assert.AreNotEqual(TransportState.Closed, transport.State);

			var newState = transport.Open();
			transport.Clear();

			Assert.AreEqual(TransportState.Open, newState);

			newState = transport.Close();

			Assert.AreEqual(TransportState.Closed, newState);
		}

        public static void TestThroughputSynchronously(IMessageTransport transport, int howManyMessages, int? maxMessagesToReceive)
		{
			var start = DateTime.Now;

			transport.Open();

            Console.WriteLine("Sending {0} messages through the {1} transport", howManyMessages, transport.GetType().FullName);

            SendMessages(transport, howManyMessages);

            Console.WriteLine("Sent {0} messages in {1} seconds", howManyMessages, DateTime.Now.Subtract(start).TotalSeconds);

			start = DateTime.Now;

			var receivedMessageCount = 0;

            var numberTimesToLoop = 1;
            if (maxMessagesToReceive.HasValue)
            {
                numberTimesToLoop = howManyMessages/maxMessagesToReceive.Value + 1;
            }
            else
            {
                maxMessagesToReceive = howManyMessages;
            }

            for (int i = 0; i < numberTimesToLoop; i++ )
            {
                foreach (var message in transport.ReceiveMany(maxMessagesToReceive.Value, TimeSpan.MaxValue))
                {
                    receivedMessageCount++;
                }

                if (howManyMessages - receivedMessageCount < maxMessagesToReceive)
                    maxMessagesToReceive = (howManyMessages - receivedMessageCount);
            }

			transport.Close();

			Console.WriteLine("Received {0} messages in {1}", receivedMessageCount, DateTime.Now.Subtract(start).TotalSeconds);
		}
        public static void TestThroughputAsynchronously(IMessageTransport transport, int howManyMessages, int howManyThreads, int? maxMessagesToReceive = null)
        {
            transport.Open();

            var start = DateTime.Now;

            var numberTimesToLoop = 1;
            if (maxMessagesToReceive.HasValue)
            {              
                var numberMessagesPerThread = howManyMessages/howManyThreads + 2;

                do
                {
                    numberMessagesPerThread--;
                    numberTimesToLoop = howManyMessages/(numberMessagesPerThread * howManyThreads) + 1;
                } while (numberMessagesPerThread > maxMessagesToReceive);

                Assert.LessOrEqual(numberMessagesPerThread, maxMessagesToReceive);
                maxMessagesToReceive = numberMessagesPerThread;
            }
            else
            {
                maxMessagesToReceive = howManyMessages/howManyThreads + 1;
            }

            Console.WriteLine("Sending {0} messages through the {1} transport across {2} threads in batches of {3}", 
                                    maxMessagesToReceive * howManyThreads * numberTimesToLoop, 
                                    transport.GetType().FullName, 
                                    howManyThreads,
                                    maxMessagesToReceive);

            for (var i = 0; i < numberTimesToLoop; i++)
            {
                var results = Parallel.For(0, howManyThreads, x =>
                {
                    SendMessages(transport, maxMessagesToReceive.Value);
                    transport.ReceiveMany(maxMessagesToReceive.Value, TimeSpan.MaxValue);
                });

            }

            Console.WriteLine("Received {0} messages in {1} seconds", (maxMessagesToReceive * howManyThreads * numberTimesToLoop), DateTime.Now.Subtract(start).TotalSeconds);

            transport.Close();
        }

        public static void TestSendingMessageOnClosedTransport(IMessageTransport transport)
        {
            transport.Open();
            transport.Close();

            var m = GetNewMessage();

            Assert.Throws(typeof (InvalidOperationException), () => transport.Send(m));
        }

        }

        private static void SendMessages(IMessageTransport transport, int numberOfMessagesToCreate)
        {
            for (var i = 0; i < numberOfMessagesToCreate; i++)
            {
                var msg = TestTransport.GetNewMessage();
                transport.Send(msg);
            }
	}
}