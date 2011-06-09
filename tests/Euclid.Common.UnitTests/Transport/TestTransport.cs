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

			for (var i = 0; i < 5; i++)
			{
				var m = new FakeMessage();
				transport.Send(m);
			}

			transport.Clear();

			var messages = transport.ReceiveMany(5, TimeSpan.MaxValue);

			Assert.AreEqual(0, messages.Count());

			transport.Close();
		}

		public static IMessage GetNewMessage()
		{
			return new FakeMessage
			       	{
			       		Identifier = Guid.NewGuid(),
			       		CallStack = "flibberty gee",
			       		Dispatched = Random.Next()%2 == 0,
			       		Error = Random.Next()%2 == 0,
			       		Field1 = Random.Next(),
			       		Field2 = new List<string>
			       		         	{
			       		         		Random.Next().ToString(),
			       		         		Random.Next().ToString(),
			       		         		Random.Next().ToString()
			       		         	}
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

		public static void TestScale(IMessageTransport transport, int howManyMessages)
		{
			var start = DateTime.Now;

			transport.Open();

			Console.WriteLine("Start transport {0} messages with the {1}", howManyMessages, transport.GetType().FullName);

			Console.WriteLine("Creating messages");

			for (var i = 0; i < howManyMessages; i++)
			{
				var msg = GetNewMessage();
				transport.Send(msg);
			}

			Console.WriteLine("Created {0} messages in {1} seconds", howManyMessages, DateTime.Now.Subtract(start).TotalSeconds);

			start = DateTime.Now;

			var receivedMessageCount = 0;

			foreach (var message in transport.ReceiveMany(howManyMessages, TimeSpan.MaxValue))
			{
				receivedMessageCount++;
			}

			transport.Close();

			Console.WriteLine("Received {0} messages in {1}", receivedMessageCount, DateTime.Now.Subtract(start).TotalSeconds);
		}
	}
}