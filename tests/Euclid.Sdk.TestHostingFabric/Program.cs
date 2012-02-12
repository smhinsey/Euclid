using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Threading;
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
using LoggingAgent.ReadModels;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using NConfig;
using log4net.Config;

namespace Euclid.Sdk.TestHostingFabric
{
	internal class Program : ILoggingSource
	{
		private Program()
		{

		}

// ReSharper disable InconsistentNaming
		private static void Main(string[] args)
// ReSharper restore InconsistentNaming
		{
			var log = new Program();
			try
			{
				NConfigurator.UsingFile(@"Config\custom.config").SetAsSystemDefault();

				XmlConfigurator.Configure(
					new FileInfo(Path.Combine(Environment.CurrentDirectory, NConfigurator.Default.FileNames[0])));

				var databaseConfiguration =
					MsSqlConfiguration.MsSql2008.ConnectionString(c => c.FromConnectionStringWithKey("test-db"));

				log.WriteInfoMessage("Starting agent console");

				var container = new WindsorContainer();

				setAzureCredentials(container, log);

				var fabric = new ConsoleFabric(container);

				var composite = new BasicCompositeApp(container)
				                	{Name = "AgentConsole Composite", Description = "The composite app used by the agent console"};

				composite.AddAgent(typeof (TestCommand).Assembly);

				composite.AddAgent(typeof (LogEntry).Assembly);

				composite.Configure(getCompositeSettings());

				composite.RegisterNh(databaseConfiguration, false);

				log.WriteInfoMessage("Initializing fabric");

				fabric.Initialize(getFabricSettings());

				log.WriteInfoMessage("Installing composite: {0}", composite.Name);

				composite.CreateSchema(databaseConfiguration, false);

				fabric.InstallComposite(composite);

				fabric.Start();

				Thread.Sleep(Timeout.Infinite);
			}
			catch (Exception e)
			{
				log.WriteFatalMessage("An exception occurred in the hosting fabric.", e);

				Console.WriteLine("Press any key to exit");
				Console.ReadKey();
			}
		}

		private static CompositeAppSettings getCompositeSettings()
		{
			var compositeAppSettings = new CompositeAppSettings();

			compositeAppSettings.BlobStorage.WithDefault(typeof (AzureBlobStorage));

			compositeAppSettings.OutputChannel.WithDefault(typeof (AzureMessageChannel));

			compositeAppSettings.CommandPublicationRecordMapper.WithDefault(typeof (NhRecordMapper<CommandPublicationRecord>));

			return compositeAppSettings;
		}

		private static FabricRuntimeSettings getFabricSettings()
		{
			var fabricSettings = new FabricRuntimeSettings();

			fabricSettings.ServiceHost.WithDefault(typeof (MultitaskingServiceHost));
			fabricSettings.HostedServices.WithDefault(new List<Type> {typeof (CommandHost)});

			var messageChannel = new AzureMessageChannel(new JsonMessageSerializer());

			fabricSettings.InputChannel.WithDefault(messageChannel);
			fabricSettings.ErrorChannel.WithDefault(messageChannel);

			return fabricSettings;
		}

		private static void setAzureCredentials(IWindsorContainer container, ILoggingSource log)
		{
			CloudStorageAccount.SetConfigurationSettingPublisher(
				(configurationKey, publishConfigurationValue) =>
					{
						var connectionString = RoleEnvironment.IsAvailable
						                       	? RoleEnvironment.GetConfigurationSettingValue(configurationKey)
						                       	: ConfigurationManager.AppSettings[configurationKey];

						publishConfigurationValue(connectionString);
					});

			var storageAccount = CloudStorageAccount.FromConfigurationSetting("DataConnectionString");

			log.WriteInfoMessage("Using Azure queue endpoint {0}", storageAccount.QueueEndpoint.AbsoluteUri);

			container.Register(Component.For<CloudStorageAccount>().Instance(storageAccount));
		}
	}
}
