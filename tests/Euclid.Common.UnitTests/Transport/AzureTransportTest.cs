using System;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Euclid.Common.Serialization;
using Euclid.Common.Transport;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using NUnit.Framework;

namespace Euclid.Common.UnitTests.Transport
{
	[TestFixture]
	public class AzureTransportTest
	{
		private IMessageSerializer _serializer;

		[TestFixtureSetUp]
		public void Setup()
		{
			CloudStorageAccount.SetConfigurationSettingPublisher((configurationKey, publishConfigurationValue) =>
			                                                     	{
			                                                     		var connectionString =
			                                                     			RoleEnvironment.IsAvailable
			                                                     				? RoleEnvironment.GetConfigurationSettingValue(
			                                                     					configurationKey)
			                                                     				: ConfigurationManager.AppSettings[configurationKey];

			                                                     		publishConfigurationValue(connectionString);
			                                                     	});

			_serializer = new JsonMessageSerializer();
		}

		private const int LargeNumber = 40;
		private const int AzureMaxReceiveAmount = 32;
		private const int NumberOfThreads = 2;

		private static void SendMessages(IMessageTransport transport, int numberOfMessagesToCreate)
		{
			for (var i = 0; i < numberOfMessagesToCreate; i++)
			{
				var msg = TestTransport.GetNewMessage();
				transport.Send(msg);
			}
		}

		[Test]
		public void TestClear()
		{
			TestTransport.Clear(new AzureMessageTransport(_serializer));
		}

		[Test]
		public void TestScaleAsynchronously()
		{
			var transport = new AzureMessageTransport(_serializer);
			transport.Open();

			var start = DateTime.Now;

			Console.WriteLine("Sending {0} messages through the {1} transport across {2} threads",
			                  NumberOfThreads*AzureMaxReceiveAmount, transport.GetType().FullName, NumberOfThreads);

			var results = Parallel.For(0, NumberOfThreads, x =>
			                                               	{
			                                               		SendMessages(transport, AzureMaxReceiveAmount);
			                                               		transport.ReceiveMany(AzureMaxReceiveAmount, TimeSpan.MaxValue);
			                                               	});

			Console.WriteLine("Received {0} messages in {1} seconds", NumberOfThreads*AzureMaxReceiveAmount,
			                  DateTime.Now.Subtract(start).TotalSeconds);

			Assert.True(results.IsCompleted);

			transport.Close();
		}

		[Test]
		public void TestScaleSynchronously()
		{
			var transport = new AzureMessageTransport(_serializer);
			transport.Open();

			var start = DateTime.Now;

			Console.WriteLine("Sending {0} messages through the {1} transport", LargeNumber, transport.GetType().FullName);

			SendMessages(transport, LargeNumber);

			Console.WriteLine("Sent {0} messages in {1} seconds", LargeNumber, DateTime.Now.Subtract(start).TotalSeconds);

			start = DateTime.Now;

			var numRequested = 0;

			var receivedMessageCount = 0;

			do
			{
				receivedMessageCount += transport.ReceiveMany(AzureMaxReceiveAmount, TimeSpan.MaxValue).Count();

				numRequested += AzureMaxReceiveAmount;
			} while (numRequested < LargeNumber);


			Console.WriteLine("Received {0} messages in {1} seconds", receivedMessageCount,
			                  DateTime.Now.Subtract(start).TotalSeconds);

			transport.Close();
		}

		[Test]
		public void TestSendReceive()
		{
			TestTransport.SendAndReceive(new AzureMessageTransport(_serializer));
		}

		[Test]
		public void TestStateTransitions()
		{
			TestTransport.StateTransitions(new AzureMessageTransport(_serializer));
		}

		[Test]
		public void TestTimeout()
		{
			TestTransport.ReceiveTimeout(new AzureMessageTransport(_serializer));
		}
	}
}