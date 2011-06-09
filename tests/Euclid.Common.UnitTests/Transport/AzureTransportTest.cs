using System;
using System.Configuration;
using Euclid.Common.TestingFakes.Transport;
using Euclid.Common.Transport;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using NUnit.Framework;

namespace Euclid.Common.UnitTests.Transport
{
    [TestFixture]
    public class AzureTransportTest
    {
        [TestFixtureSetUp]
        public void Setup()
        {
            CloudStorageAccount.SetConfigurationSettingPublisher((configurationKey, publishConfigurationValue) =>
            {
                var connectionString =
                    RoleEnvironment.IsAvailable
                          ? RoleEnvironment.GetConfigurationSettingValue(configurationKey)
                          : ConfigurationManager.AppSettings[configurationKey];

                publishConfigurationValue(connectionString);
            });
        }

        [Test]
        public void TestStateTransitions()
        {
            TestTransport.StateTransitions(new AzureMessageTransport());
        }

        [Test]
        public void TestSendReceive()
        {
            TestTransport.SendAndReceive(new AzureMessageTransport());
        }

        [Test]
        public void TestTimeout()
        {
            TestTransport.ReceiveTimeout(new AzureMessageTransport());
        }

        [Test]
        public void TestClear()
        {
            TestTransport.Clear(new AzureMessageTransport());
        }

        [Test]
        public void TestDelete()
        {
            TestTransport.Delete(new AzureMessageTransport());
        }

        [Test]
        public void TestPeek()
        {
            TestTransport.Peek(new AzureMessageTransport());
        }

    }
}
