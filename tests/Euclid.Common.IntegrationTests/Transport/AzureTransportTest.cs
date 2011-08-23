using System.Configuration;
using Euclid.Common.Messaging;
using Euclid.Common.Messaging.Azure;
using Euclid.Common.UnitTests.Transport;
using Euclid.TestingSupport;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using NUnit.Framework;

namespace Euclid.Common.IntegrationTests.Transport
{
	[TestFixture]
	[Category(TestCategories.Integration)]
	public class AzureTransportTest
	{
		private const int AzureMaxReceiveAmount = 32;

		private const int LargeNumber = 125;

		private const int NumberOfThreads = 10;

		private IMessageSerializer _serializer;

		[TestFixtureSetUp]
		public void Setup()
		{
			CloudStorageAccount.SetConfigurationSettingPublisher(
				(configurationKey, publishConfigurationValue) =>
					{
						var connectionString = RoleEnvironment.IsAvailable
						                       	? RoleEnvironment.GetConfigurationSettingValue(configurationKey)
						                       	: ConfigurationManager.AppSettings[configurationKey];

						publishConfigurationValue(connectionString);
					});

			_serializer = new JsonMessageSerializer();
		}

		[Test]
		public void TestClear()
		{
			TestTransport.Clear(new AzureMessageChannel(_serializer));
		}

		[Test]
		public void TestScaleAsynchronously()
		{
			TestTransport.TestThroughputAsynchronously(
				new AzureMessageChannel(_serializer), LargeNumber, NumberOfThreads, AzureMaxReceiveAmount);
		}

		[Test]
		public void TestScaleSynchronously()
		{
			TestTransport.TestThroughputSynchronously(new AzureMessageChannel(_serializer), LargeNumber, AzureMaxReceiveAmount);
		}

		[Test]
		public void TestSendReceive()
		{
			TestTransport.SendAndReceiveSingleMessage(new AzureMessageChannel(_serializer));
		}

		[Test]
		public void TestSendingMessageOnClosedTransport()
		{
			TestTransport.TestSendingMessageOnClosedTransport(new AzureMessageChannel(_serializer));
		}

		[Test]
		public void TestStateTransitions()
		{
			TestTransport.StateTransitions(new AzureMessageChannel(_serializer));
		}

		[Test]
		public void TestTimeout()
		{
			TestTransport.ReceiveTimeout(new AzureMessageChannel(_serializer));
		}

		private static void SendMessages(IMessageChannel channel, int numberOfMessagesToCreate)
		{
			for (var i = 0; i < numberOfMessagesToCreate; i++)
			{
				var msg = TestTransport.GetNewMessage();
				channel.Send(msg);
			}
		}
	}
}