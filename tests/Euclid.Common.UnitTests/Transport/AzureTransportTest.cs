using System.Configuration;
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

		private const int LargeNumber = 125;
		private const int AzureMaxReceiveAmount = 32;
		private const int NumberOfThreads = 10;

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
			TestTransport.TestThroughputAsynchronously(new AzureMessageTransport(_serializer), LargeNumber, NumberOfThreads,
			                                           AzureMaxReceiveAmount);
		}

		[Test]
		public void TestScaleSynchronously()
		{
			TestTransport.TestThroughputSynchronously(new AzureMessageTransport(_serializer), LargeNumber, AzureMaxReceiveAmount);
		}

		[Test]
		public void TestSendReceive()
		{
			TestTransport.SendAndReceiveSingleMessage(new AzureMessageTransport(_serializer));
		}

		[Test]
		public void TestSendingMessageOnClosedTransport()
		{
			TestTransport.TestSendingMessageOnClosedTransport(new AzureMessageTransport(_serializer));
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