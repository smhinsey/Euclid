using System;
using System.Collections.Generic;
using AgentConsole;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Euclid.Common.Messaging;
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

			composite.RegisterNh(SQLiteConfiguration.Standard.UsingFile("HostingFabricFixtureDb"), true, false);

			composite.AddAgent(typeof (PublishPost).Assembly);

			composite.Configure(getCompositeSettings());

			Fabric.Initialize(getFabricSettings());

			Fabric.InstallComposite(composite);

			Fabric.Start();
		}

		private CompositeAppSettings getCompositeSettings()
		{
			var compositeAppSettings = new CompositeAppSettings();

			compositeAppSettings.BlobStorage.WithDefault(typeof (AzureBlobStorage));
			compositeAppSettings.CommandPublicationRecordMapper.WithDefault(typeof (NhRecordMapper<CommandPublicationRecord>));

			return compositeAppSettings;
		}

		private FabricRuntimeSettings getFabricSettings()
		{
			var fabricSettings = new FabricRuntimeSettings();

			fabricSettings.ServiceHost.WithDefault(typeof (MultitaskingServiceHost));
			fabricSettings.HostedServices.WithDefault(new List<Type> {typeof (CommandHost)});

			fabricSettings.InputChannel.WithDefault(new InMemoryMessageChannel());
			fabricSettings.ErrorChannel.WithDefault(new InMemoryMessageChannel());

			return fabricSettings;
		}

		private void setAzureCredentials(IWindsorContainer container)
		{
			var storageAccount = new CloudStorageAccount(CloudStorageAccount.DevelopmentStorageAccount.Credentials,
			                                             CloudStorageAccount.DevelopmentStorageAccount.BlobEndpoint,
			                                             CloudStorageAccount.DevelopmentStorageAccount.QueueEndpoint,
			                                             CloudStorageAccount.DevelopmentStorageAccount.TableEndpoint);

			container.Register(Component.For<CloudStorageAccount>().Instance(storageAccount));
		}
	}
}