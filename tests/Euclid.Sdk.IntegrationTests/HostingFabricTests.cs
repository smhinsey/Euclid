﻿using System;
using System.Collections.Generic;
using System.Threading;
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
using Euclid.Sdk.FakeAgent.Commands;
using FluentNHibernate.Cfg.Db;
using Microsoft.WindowsAzure;
using NUnit.Framework;
using log4net.Config;

namespace Euclid.Sdk.IntegrationTests
{
	public class HostingFabricTests
	{
		private WindsorContainer _container;
		private ConsoleFabric _fabric;

		[Test]
		public void PublishManyCommands()
		{
			var publisher = _container.Resolve<IPublisher>();

			var publicationIds = new List<Guid>();

			for (var i = 0; i < 10; i++)
			{
				var publicationId = publisher.PublishMessage(new FakeCommand {Number = i});

				publicationIds.Add(publicationId);
			}

			var registry = _container.Resolve<ICommandRegistry>();

			Thread.Sleep(15000);

			foreach (var publicationId in publicationIds)
			{
				var record = registry.GetRecord(publicationId);

				Assert.IsTrue(record.Completed, "Publication record was marked complete.");
			}
		}

		[SetUp]
		public void SetUp()
		{
			XmlConfigurator.Configure();

			_container = new WindsorContainer();

			setAzureCredentials(_container);

			_fabric = new ConsoleFabric(_container);

			var composite = new BasicCompositeApp(_container);

			composite.RegisterNh(SQLiteConfiguration.Standard.UsingFile("AgentConsoleDb"), true, false);

			composite.AddAgent(typeof (FakeCommand).Assembly);

			composite.Configure(getCompositeSettings());

			_fabric.Initialize(getFabricSettings());

			_fabric.InstallComposite(composite);

			_fabric.Start();
		}

		private static CompositeAppSettings getCompositeSettings()
		{
			var compositeAppSettings = new CompositeAppSettings();

			compositeAppSettings.BlobStorage.WithDefault(typeof (AzureBlobStorage));
			compositeAppSettings.CommandPublicationRecordMapper.WithDefault(typeof (NhRecordMapper<CommandPublicationRecord>));

			return compositeAppSettings;
		}

		private static FabricRuntimeSettings getFabricSettings()
		{
			var fabricSettings = new FabricRuntimeSettings();

			fabricSettings.ServiceHost.WithDefault(typeof (MultitaskingServiceHost));
			fabricSettings.HostedServices.WithDefault(new List<Type> {typeof (CommandHost)});

			fabricSettings.InputChannel.WithDefault(new InMemoryMessageChannel());
			fabricSettings.ErrorChannel.WithDefault(new InMemoryMessageChannel());

			return fabricSettings;
		}

		private static void setAzureCredentials(IWindsorContainer container)
		{
			var storageAccount = new CloudStorageAccount(CloudStorageAccount.DevelopmentStorageAccount.Credentials,
			                                             CloudStorageAccount.DevelopmentStorageAccount.BlobEndpoint,
			                                             CloudStorageAccount.DevelopmentStorageAccount.QueueEndpoint,
			                                             CloudStorageAccount.DevelopmentStorageAccount.TableEndpoint);

			container.Register(Component.For<CloudStorageAccount>().Instance(storageAccount));
		}
	}
}