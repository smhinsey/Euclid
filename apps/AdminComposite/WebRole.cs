using System.Web.Mvc;
using AdminComposite.Converters;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using CommonServiceLocator.WindsorAdapter;
using Euclid.Common.Messaging.Azure;
using Euclid.Common.Storage.Azure;
using Euclid.Common.Storage.NHibernate;
using Euclid.Composites;
using Euclid.Composites.Mvc;
using Euclid.Framework.Cqrs;
using FluentNHibernate.Cfg.Db;
using ForumAgent.Commands;
using LoggingAgent.Queries;
using Microsoft.WindowsAzure;
using log4net.Config;

namespace AdminComposite
{
	public class WebRole
	{
		private static WebRole _instance;

		private bool _initialized;

		private WebRole()
		{
		}

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

			XmlConfigurator.Configure();

			var databaseConfiguration = MsSqlConfiguration.MsSql2008.ConnectionString(c => c.FromConnectionStringWithKey("test-db"));

			var container = new WindsorContainer();

			var composite = new MvcCompositeApp(container)
				{ Name = "Admin", Description = "Create and manage custom forums." };

			composite.RegisterNh(databaseConfiguration, true);

			var compositeAppSettings = new CompositeAppSettings();

			compositeAppSettings.OutputChannel.ApplyOverride(typeof(AzureMessageChannel));
			compositeAppSettings.BlobStorage.WithDefault(typeof(AzureBlobStorage));
			compositeAppSettings.CommandPublicationRecordMapper.WithDefault(typeof(NhRecordMapper<CommandPublicationRecord>));

			composite.Configure(compositeAppSettings);

			composite.AddAgent(typeof(PublishPost).Assembly);
			composite.AddAgent(typeof(LogQueries).Assembly);

			composite.RegisterInputModel(new CreateForumInputModelConverter());

			setAzureCredentials(container);

			_initialized = true;

			DependencyResolver.SetResolver(new WindsorServiceLocator(container));
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