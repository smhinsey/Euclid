using System;
using System.Collections.Generic;
using System.Reflection;
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
using Microsoft.WindowsAzure;
using log4net.Config;

namespace Euclid.TestingSupport
{
	public class AgentConfigurator
	{
		private readonly IWindsorContainer _container;

		private bool _configured;

		public AgentConfigurator(IWindsorContainer container)
		{
			_container = container;
		}

		public ConsoleFabric Fabric { get; set; }

		public void Configure(Assembly agentAssembly)
		{
			if (_configured)
			{
				return;
			}

			var compositeDatabaseConnection =
				MsSqlConfiguration.MsSql2008.ConnectionString(c => c.FromConnectionStringWithKey("test-db"));

			XmlConfigurator.Configure();

			setAzureCredentials(_container);

			Fabric = new ConsoleFabric(_container);

			var composite = new BasicCompositeApp(_container)
				{ Name = "Euclid.TestingSupport.ConfigureAgentSteps.Composite", Description = "A composite used for testing" };

			composite.AddAgent(agentAssembly);

			composite.Configure(getCompositeSettings());

			composite.RegisterNh(compositeDatabaseConnection, false);

			Fabric.Initialize(getFabricSettings());

			Fabric.InstallComposite(composite);

			Fabric.Start();

			_container.Register(Component.For<BasicFabric>().Instance(Fabric));

			composite.CreateSchema(compositeDatabaseConnection, true);

			_configured = true;
		}

		private CompositeAppSettings getCompositeSettings()
		{
			var compositeAppSettings = new CompositeAppSettings();

			compositeAppSettings.OutputChannel.WithDefault(typeof(AzureMessageChannel));
			compositeAppSettings.BlobStorage.WithDefault(typeof(AzureBlobStorage));
			compositeAppSettings.CommandPublicationRecordMapper.WithDefault(typeof(NhRecordMapper<CommandPublicationRecord>));

			return compositeAppSettings;
		}

		private FabricRuntimeSettings getFabricSettings()
		{
			var fabricSettings = new FabricRuntimeSettings();

			fabricSettings.ServiceHost.WithDefault(typeof(MultitaskingServiceHost));
			fabricSettings.HostedServices.WithDefault(new List<Type> { typeof(CommandHost) });

			var messageChannel = new AzureMessageChannel(new JsonMessageSerializer());

			fabricSettings.InputChannel.WithDefault(messageChannel);
			fabricSettings.ErrorChannel.WithDefault(messageChannel);

			return fabricSettings;
		}

		private void setAzureCredentials(IWindsorContainer container)
		{
			// as soon as we can stop using the azure storage emulator we should
			var storageAccount = new CloudStorageAccount(
				CloudStorageAccount.DevelopmentStorageAccount.Credentials,
				CloudStorageAccount.DevelopmentStorageAccount.BlobEndpoint,
				CloudStorageAccount.DevelopmentStorageAccount.QueueEndpoint,
				CloudStorageAccount.DevelopmentStorageAccount.TableEndpoint);

			container.Register(Component.For<CloudStorageAccount>().Instance(storageAccount));
		}
	}
}