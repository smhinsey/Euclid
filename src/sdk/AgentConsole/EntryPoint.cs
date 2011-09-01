﻿using System;
using System.Collections.Generic;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Euclid.Common.Logging;
using Euclid.Common.Messaging;
using Euclid.Common.Messaging.Azure;
using Euclid.Common.ServiceHost;
using Euclid.Common.Storage.Azure;
using Euclid.Common.Storage.NHibernate;
using Euclid.Composites;
using Euclid.Framework.Cqrs;
using Euclid.Framework.HostingFabric;
using Euclid.Sdk.TestAgent.Commands;
using FluentNHibernate.Cfg.Db;
using ForumAgent.Commands;
using LoggingAgent.Queries;
using Microsoft.WindowsAzure;
using log4net.Config;

namespace AgentConsole
{
	public class EntryPoint : ILoggingSource
	{
		private static EntryPoint _instance;

		private EntryPoint()
		{
			
		}

		public static void Main(string[] args)
		{
			if (_instance == null)
			{
				_instance = new EntryPoint();
			}


			var databaseConfiguration =
				MsSqlConfiguration.MsSql2008.ConnectionString(c => c.FromConnectionStringWithKey("test-db"));

			try
			{
				XmlConfigurator.Configure();

				_instance.WriteInfoMessage("Starting agent console");

				var container = new WindsorContainer();

				setAzureCredentials(container);

				var fabric = new ConsoleFabric(container);

				var composite = new BasicCompositeApp(container)
					{ Name = "AgentConsole Composite", Description = "The composite app used by the agent console" };

				composite.AddAgent(typeof(PublishPost).Assembly);

				// composite.AddAgent(typeof(TestCommand).Assembly);

				composite.Configure(getCompositeSettings());

				composite.RegisterNh(databaseConfiguration, false);

				_instance.WriteInfoMessage("Initializing fabric");

				fabric.Initialize(getFabricSettings());

				_instance.WriteInfoMessage("Installing composite: {0}", composite.Name);
				
				composite.CreateSchema(databaseConfiguration);

				fabric.InstallComposite(composite);

				fabric.Start();

				Console.ReadLine();
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				Console.ReadLine();
			}
		}

		private static CompositeAppSettings getCompositeSettings()
		{
			var compositeAppSettings = new CompositeAppSettings();

			compositeAppSettings.BlobStorage.WithDefault(typeof(AzureBlobStorage));

			compositeAppSettings.OutputChannel.WithDefault(typeof(AzureMessageChannel));

			compositeAppSettings.CommandPublicationRecordMapper.WithDefault(typeof(NhRecordMapper<CommandPublicationRecord>));

			return compositeAppSettings;
		}

		private static FabricRuntimeSettings getFabricSettings()
		{
			var fabricSettings = new FabricRuntimeSettings();

			fabricSettings.ServiceHost.WithDefault(typeof(MultitaskingServiceHost));
			fabricSettings.HostedServices.WithDefault(new List<Type> { typeof(CommandHost) });

			var messageChannel = new AzureMessageChannel(new JsonMessageSerializer());

			messageChannel.Open();

			messageChannel.Clear();

			messageChannel.Close();

			fabricSettings.InputChannel.WithDefault(messageChannel);
			fabricSettings.ErrorChannel.WithDefault(messageChannel);

			return fabricSettings;
		}

		private static void setAzureCredentials(IWindsorContainer container)
		{
			var storageAccount = new CloudStorageAccount(
				CloudStorageAccount.DevelopmentStorageAccount.Credentials,
				CloudStorageAccount.DevelopmentStorageAccount.BlobEndpoint,
				CloudStorageAccount.DevelopmentStorageAccount.QueueEndpoint,
				CloudStorageAccount.DevelopmentStorageAccount.TableEndpoint);

			container.Register(Component.For<CloudStorageAccount>().Instance(storageAccount));
		}

		public string Name
		{
			get { return "AgentConsole"; }
		}
	}
}