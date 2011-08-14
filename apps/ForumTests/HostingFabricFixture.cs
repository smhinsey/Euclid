using System;
using System.Collections.Generic;
using AgentConsole;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Euclid.Common.Messaging;
using Euclid.Common.Messaging.Azure;
using Euclid.Common.ServiceHost;
using Euclid.Common.Storage.Azure;
using Euclid.Common.Storage.NHibernate;
using Euclid.Composites;
using Euclid.Framework.Cqrs;
using Euclid.Framework.HostingFabric;
using FluentNHibernate.Cfg.Db;
using ForumAgent.Commands;
using Microsoft.WindowsAzure;
using NUnit.Framework;
using log4net.Config;

namespace ForumTests
{
	// SELF this needs to be put somewhere else, but i'm not sure it really goes in Common, which is the "logical" place
	public class HostingFabricFixture
	{
		protected WindsorContainer Container;
		protected ConsoleFabric Fabric;

		[SetUp]
		public void SetUp()
		{
			XmlConfigurator.Configure();

			Container = new WindsorContainer();

			setAzureCredentials(Container);

			Fabric = new ConsoleFabric(Container);

			var composite = new BasicCompositeApp(Container);

			composite.RegisterNh(MsSqlConfiguration.MsSql2008.ConnectionString(c => c.FromConnectionStringWithKey("test-db")), true, false);

			composite.AddAgent(typeof (PublishPost).Assembly);

			composite.Configure(getCompositeSettings());

			Fabric.Initialize(getFabricSettings());

			Fabric.InstallComposite(composite);

			Fabric.Start();
		}

		private CompositeAppSettings getCompositeSettings()
		{
			var compositeAppSettings = new CompositeAppSettings();

			compositeAppSettings.MessageChannel.WithDefault(typeof(AzureMessageChannel));
			compositeAppSettings.BlobStorage.WithDefault(typeof (AzureBlobStorage));
			compositeAppSettings.CommandPublicationRecordMapper.WithDefault(typeof (NhRecordMapper<CommandPublicationRecord>));

			return compositeAppSettings;
		}

		private FabricRuntimeSettings getFabricSettings()
		{
			var fabricSettings = new FabricRuntimeSettings();

			fabricSettings.ServiceHost.WithDefault(typeof (MultitaskingServiceHost));
			fabricSettings.HostedServices.WithDefault(new List<Type> {typeof (CommandHost)});

			var messageChannel = new AzureMessageChannel(new JsonMessageSerializer());

			fabricSettings.InputChannel.WithDefault(messageChannel);
			fabricSettings.ErrorChannel.WithDefault(messageChannel);

			return fabricSettings;
		}

		private void setAzureCredentials(IWindsorContainer container)
		{
			// as soon as we can stop using the azure storage emulator we should

			var storageAccount = new CloudStorageAccount(CloudStorageAccount.DevelopmentStorageAccount.Credentials,
			                                             CloudStorageAccount.DevelopmentStorageAccount.BlobEndpoint,
			                                             CloudStorageAccount.DevelopmentStorageAccount.QueueEndpoint,
			                                             CloudStorageAccount.DevelopmentStorageAccount.TableEndpoint);

			container.Register(Component.For<CloudStorageAccount>().Instance(storageAccount));
		}
	}
}