using System;
using System.Configuration;
using System.IO;
using System.Linq;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Euclid.Common.Logging;
using Euclid.Common.Messaging.Azure;
using Euclid.Common.Storage.Azure;
using Euclid.Common.Storage.NHibernate;
using Euclid.Composites;
using Euclid.Composites.Mvc;
using Euclid.Framework.Cqrs;
using FluentNHibernate.Cfg.Db;
using LoggingAgent.Queries;
using Microsoft.Web.Administration;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using NConfig;
using StorefrontAgent.Commands;
using log4net.Config;

namespace StorefrontAdminComposite
{
	public class WebRole : RoleEntryPoint, ILoggingSource
	{
		private static WebRole _instance;

		private bool _initialized;

		public static WebRole GetInstance()
		{
			return _instance ?? (_instance = new WebRole());
		}

		public void Init()
		{
			if (_initialized)
			{
				return;
			}

			if (!RoleEnvironment.IsAvailable)
			{
				NConfigurator.UsingFile(@"~\Config\custom.config").SetAsSystemDefault();
				XmlConfigurator.Configure(new FileInfo(Path.Combine(Environment.CurrentDirectory, NConfigurator.Default.FileNames[0])));
			}
			else
			{
				XmlConfigurator.Configure();
			}

			var databaseConfiguration =
				MsSqlConfiguration.MsSql2008.ConnectionString(c => c.FromConnectionStringWithKey("storefront-db"));

			var container = new WindsorContainer();

			var composite = new MvcCompositeApp(container) { Name = "Newco Storefront Admin", Description = "Create and manage custom storefronts." };

			composite.RegisterNh(databaseConfiguration, true);

			var compositeAppSettings = new CompositeAppSettings();

			compositeAppSettings.OutputChannel.WithDefault(typeof(AzureMessageChannel));
			compositeAppSettings.BlobStorage.WithDefault(typeof(AzureBlobStorage));
			compositeAppSettings.CommandPublicationRecordMapper.WithDefault(typeof(NhRecordMapper<CommandPublicationRecord>));

			composite.Configure(compositeAppSettings);

			composite.CreateSchema(databaseConfiguration, false);

			composite.AddAgent(typeof(RegisterNewCompany).Assembly);
			composite.AddAgent(typeof(LogQueries).Assembly);

			// register input model maps

			setAzureCredentials(container);

			_initialized = true;
		}

		public override bool OnStart()
		{
			using (var serverManager = new ServerManager())
			{
				var siteName = RoleEnvironment.CurrentRoleInstance.Id + "_Web";

				var siteApplication = serverManager.Sites[siteName].Applications.First();
				var appPoolName = siteApplication.ApplicationPoolName;

				var appPool = serverManager.ApplicationPools[appPoolName];

				appPool.ProcessModel.IdleTimeout = TimeSpan.Zero;
				appPool.Recycling.PeriodicRestart.Time = TimeSpan.Zero;

				serverManager.CommitChanges();
			}

			return base.OnStart();
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