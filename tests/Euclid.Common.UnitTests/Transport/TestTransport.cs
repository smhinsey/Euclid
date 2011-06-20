using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Euclid.Common.Messaging;
using Euclid.Common.TestingFakes.Transport;
using NUnit.Framework;

namespace Euclid.Common.UnitTests.Transport
{
	public class TestTransport
	{
		private static readonly Random Random = new Random((int) DateTime.Now.Ticks);

		public static void Clear(IMessageChannel channel)
		{
			channel.Open();

			for (var i = 0; i < 5; i++)
			{
				var m = new FakeMessage();
				channel.Send(m);
			}

			channel.Clear();

			var messages = channel.ReceiveMany(5, TimeSpan.MaxValue);

			Assert.AreEqual(0, messages.Count());

			channel.Close();
		}

		public static IMessage GetNewMessage()
		{
			return new FakeMessage
			       	{
			       		Identifier = Guid.NewGuid(),
			       		Field1 = Random.Next(),
			       		Field2 = new List<string>
			       		         	{
			       		         		Random.Next().ToString(),
			       		         		Random.Next().ToString(),
			       		         		Random.Next().ToString()
			       		         	}
			       	};
		}

		public static void ReceiveTimeout(IMessageChannel channel)
		{
			var ts = new TimeSpan(0, 0, 0, 0, 500);

			channel.Open();
			channel.Clear();

			var m = new FakeMessage();
			var m2 = new FakeMessage();

			channel.Send(m);
			channel.Send(m2);

			var count = 0;
			foreach (var msg in channel.ReceiveMany(2, ts))
			{
				count++;
				Thread.Sleep(500);
			}

			Assert.AreEqual(1, count);
		}

		public static void SendAndReceiveSingleMessage(IMessageChannel channel)
		{
			channel.Open();
			channel.Clear();

			var m = GetNewMessage();

			channel.Send(m);

			var m2 = channel.ReceiveSingle(TimeSpan.MaxValue);

			Assert.NotNull(m2);

			Assert.AreEqual(m.Identifier, m2.Identifier);

			channel.Close();
		}

		public static void StateTransitions(IMessageChannel channel)
		{
			Assert.AreNotEqual(ChannelState.Closed, channel.State);

			var newState = channel.Open();
			channel.Clear();

			Assert.AreEqual(ChannelState.Open, newState);

			newState = channel.Close();

			Assert.AreEqual(ChannelState.Closed, newState);
		}

		public static void TestRetrievingSpecificMessages(IMessageChannel channel, int maxNumberOfMessages = 100)
		{
			var r = new Random();
			var fakeMessageCount = 0;
			var differentMessageCount = 0;

			channel.Open();

			for (var i = 0; i < maxNumberOfMessages; i++)
			{
				IMessage msg;

				if (r.Next()%2 == 0)
				{
					msg = new FakeMessage();
					fakeMessageCount++;
				}
				else
				{
					msg = new DifferentFakeMessage();
					differentMessageCount++;
				}

				channel.Send(msg);
			}

			var differentMessages = channel.ReceiveMany<DifferentFakeMessage>(differentMessageCount, TimeSpan.MaxValue);

			Assert.AreEqual(differentMessageCount, differentMessages.Count());

			differentMessages = channel.ReceiveMany<DifferentFakeMessage>(differentMessageCount, TimeSpan.MaxValue);

			Assert.AreEqual(0, differentMessages.Count());

			channel.Close();
		}

		public static void TestSendingMessageOnClosedTransport(IMessageChannel channel)
		{
			channel.Open();
			channel.Close();

			var m = GetNewMessage();

			Assert.Throws(typeof (InvalidOperationException), () => channel.Send(m));
		}

		public static void TestThroughputAsynchronously
			(IMessageChannel channel, int howManyMessages, int howManyThreads,
			 int? maxMessagesToReceive = null)
		{
			channel.Open();

			var start = DateTime.Now;

			var numberTimesToLoop = 1;
			if (maxMessagesToReceive.HasValue)
			{
				var numberMessagesPerThread = howManyMessages/howManyThreads + 2;

				do
				{
					numberMessagesPerThread--;
					numberTimesToLoop = howManyMessages/(numberMessagesPerThread*howManyThreads) + 1;
				} while (numberMessagesPerThread > maxMessagesToReceive);

				Assert.LessOrEqual(numberMessagesPerThread, maxMessagesToReceive);
				maxMessagesToReceive = numberMessagesPerThread;
			}
			else
			{
				maxMessagesToReceive = howManyMessages/howManyThreads + 1;
			}

			Console.WriteLine
				("Sending {0} messages through the {1} channel across {2} threads in batches of {3}",
				 maxMessagesToReceive*howManyThreads*numberTimesToLoop,
				 channel.GetType().FullName,
				 howManyThreads,
				 maxMessagesToReceive);

			for (var i = 0; i < numberTimesToLoop; i++)
			{
				var results = Parallel.For
					(0, howManyThreads, x =>
					                    	{
					                    		SendMessages(channel, maxMessagesToReceive.Value);
					                    		channel.ReceiveMany(maxMessagesToReceive.Value, TimeSpan.MaxValue);
					                    	});

				Assert.True(results.IsCompleted);
			}

			Console.WriteLine
				("Received {0} messages in {1} seconds", (maxMessagesToReceive*howManyThreads*numberTimesToLoop),
				 DateTime.Now.Subtract(start).TotalSeconds);

			channel.Close();
		}

		public static void TestThroughputSynchronously
			(IMessageChannel channel, int howManyMessages,
			 int? maxMessagesToReceive)
		{
			var start = DateTime.Now;

			channel.Open();

			Console.WriteLine("Sending {0} messages through the {1} channel", howManyMessages, channel.GetType().FullName);

			SendMessages(channel, howManyMessages);

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

			for (var i = 0; i < numberTimesToLoop; i++)
			{
				foreach (var message in channel.ReceiveMany(maxMessagesToReceive.Value, TimeSpan.MaxValue))
				{
					receivedMessageCount++;
				}

				if (howManyMessages - receivedMessageCount < maxMessagesToReceive)
					maxMessagesToReceive = (howManyMessages - receivedMessageCount);
			}

			channel.Close();

			Console.WriteLine("Received {0} messages in {1}", receivedMessageCount, DateTime.Now.Subtract(start).TotalSeconds);
		}

		private static void SendMessages(IMessageChannel channel, int numberOfMessagesToCreate)
		{
			for (var i = 0; i < numberOfMessagesToCreate; i++)
			{
				var msg = GetNewMessage();
				channel.Send(msg);
			}
		}
	}
}