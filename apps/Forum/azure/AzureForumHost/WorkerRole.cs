using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
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
using FluentNHibernate.Cfg.Db;
using ForumAgent.Commands;
using LoggingAgent.ReadModels;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using NConfig;
using log4net.Config;

namespace AzureAgentHost
{
	public class WorkerRole : RoleEntryPoint, ILoggingSource
	{
		public override bool OnStart()
		{
			// Set the maximum number of concurrent connections 
			ServicePointManager.DefaultConnectionLimit = 12;

			// For information on handling configuration changes
			// see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

			return base.OnStart();
		}

		public override void Run()
		{
			try
			{
				if (!RoleEnvironment.IsAvailable)
				{
					NConfigurator.UsingFile(@"~\Config\custom.config").SetAsSystemDefault();
					XmlConfigurator.Configure(
						new FileInfo(Path.Combine(Environment.CurrentDirectory, NConfigurator.Default.FileNames[0])));
					;
				}
				else
				{
					XmlConfigurator.Configure();
				}

				var databaseConfiguration =
					MsSqlConfiguration.MsSql2008.ConnectionString(c => c.FromConnectionStringWithKey("forum-db"));

				this.WriteInfoMessage("Starting agent console");

				var container = new WindsorContainer();

				setAzureCredentials(container);

				var fabric = new ConsoleFabric(container);

				var composite = new BasicCompositeApp(container)
					{ Name = "AgentConsole Composite", Description = "The composite app used by the agent console" };

				composite.AddAgent(typeof(PublishPost).Assembly);

				composite.AddAgent(typeof(LogEntry).Assembly);

				composite.Configure(getCompositeSettings());

				composite.RegisterNh(databaseConfiguration, false);

				this.WriteInfoMessage("Initializing fabric");

				fabric.Initialize(getFabricSettings());

				this.WriteInfoMessage("Installing composite: {0}", composite.Name);

				composite.CreateSchema(databaseConfiguration, false);

				fabric.InstallComposite(composite);

				fabric.Start();

				Thread.Sleep(Timeout.Infinite);
			}
			catch (Exception e)
			{
				this.WriteFatalMessage("WorkerRole failed and will recycle.", e);
			}

			this.WriteInfoMessage("Worker role is recycling.");
		}

		private CompositeAppSettings getCompositeSettings()
		{
			var compositeAppSettings = new CompositeAppSettings();

			compositeAppSettings.BlobStorage.WithDefault(typeof(AzureBlobStorage));

			compositeAppSettings.OutputChannel.WithDefault(typeof(AzureMessageChannel));

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
			CloudStorageAccount.SetConfigurationSettingPublisher(
				(configurationKey, publishConfigurationValue) =>
				{
					var connectionString = RoleEnvironment.IsAvailable
																	? RoleEnvironment.GetConfigurationSettingValue(configurationKey)
																	: ConfigurationManager.AppSettings[configurationKey];

					publishConfigurationValue(connectionString);
				});

			var storageAccount = CloudStorageAccount.FromConfigurationSetting("DataConnectionString");

			this.WriteInfoMessage("Using Azure queue endpoint {0}", storageAccount.QueueEndpoint.AbsoluteUri);

			container.Register(Component.For<CloudStorageAccount>().Instance(storageAccount));
		}
	}
}