using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
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
using NUnit.Framework;
using log4net.Config;

namespace Euclid.TestingSupport
{
	public class HostingFabricFixture
	{
		protected WindsorContainer Container;

		protected ConsoleFabric Fabric;

		private readonly Assembly[] _agentAssemblies;

		public HostingFabricFixture(params Assembly[] agentAssemblies)
		{
			_agentAssemblies = agentAssemblies;
		}

		[SetUp]
		public void SetUp()
		{
			var compositeDatabaseConnection =
				MsSqlConfiguration.MsSql2008.ConnectionString(c => c.FromConnectionStringWithKey("test-db"));

			XmlConfigurator.Configure();

			Container = new WindsorContainer();

			setAzureCredentials(Container);

			Fabric = new ConsoleFabric(Container);

			var composite = new BasicCompositeApp(Container)
				{
					Name = "Euclid.TestingSupport.HostingFabricFixture.Composite",
					Description = "A composite used in specification tests"
				};

			composite.RegisterNh(compositeDatabaseConnection, true, false);

			foreach (var agentAssembly in _agentAssemblies)
			{
				composite.AddAgent(agentAssembly);
			}

			composite.Configure(getCompositeSettings());

			Fabric.Initialize(getFabricSettings());

			Fabric.InstallComposite(composite);

			composite.CreateSchema(compositeDatabaseConnection);

			Fabric.Start();
		}

		protected void WaitUntilComplete(Guid publicationId)
		{
			while (true)
			{
				var registry = Container.Resolve<ICommandRegistry>();

				var record = registry.GetPublicationRecord(publicationId);

				if (record.Completed || record.Error)
				{
					break;
				}

				Thread.Sleep(250);
			}
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