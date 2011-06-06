using System;
using System.Collections.Generic;
using Euclid.Common.TestingFakes.Transport;
using Euclid.Common.Transport;
using NUnit.Framework;

namespace Euclid.Common.UnitTests.Transport
{
	[TestFixture]
	public abstract class TransportTester
	{
		public abstract ITransport GetTransport();

		[Test]
		public void TestSendReceive()
		{
			var transport = GetTransport();
			var ids = new List<Guid>();

			transport.Open();

			for (var i = 0; i < 100; i++)
			{
				var message = new FakeMessage();
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

		[Test]
		public void TestTransportStateTransitions()
		{
			var transport = GetTransport();

			Assert.AreEqual(TransportState.Invalid, transport.State);

			var newState = transport.Open();
			Assert.AreEqual(TransportState.Open, newState);

			newState = transport.Close();
			Assert.AreEqual(TransportState.Closed, newState);
		}
	}

	public class InMemoryTransportTest : TransportTester
	{
		public override ITransport GetTransport()
		{
			return new InMemoryTransport();
		}
	}
}