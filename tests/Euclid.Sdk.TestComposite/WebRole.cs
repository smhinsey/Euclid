using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Euclid.Common.Messaging.Azure;
using Euclid.Common.Storage.Azure;
using Euclid.Common.Storage.NHibernate;
using Euclid.Composites;
using Euclid.Composites.Mvc;
using Euclid.Framework.Cqrs;
using Euclid.Sdk.TestAgent.Commands;
using Euclid.Sdk.TestComposite.Models;
using FluentNHibernate.Cfg.Db;
using LoggingAgent.Queries;
using Microsoft.WindowsAzure;

namespace Euclid.Sdk.TestComposite
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

			var compositeDatabaseConnection =
				MsSqlConfiguration.MsSql2008.ConnectionString(c => c.FromConnectionStringWithKey("test-db"));

			var container = new WindsorContainer();

			var composite = new MvcCompositeApp(container)
				{ Name = "Test Composite", Description = "A composite application that is used to validate the Euclid platform" };

			var compositeAppSettings = new CompositeAppSettings();

			compositeAppSettings.OutputChannel.ApplyOverride(typeof(AzureMessageChannel));
			compositeAppSettings.BlobStorage.WithDefault(typeof(AzureBlobStorage));
			compositeAppSettings.CommandPublicationRecordMapper.WithDefault(typeof(NhRecordMapper<CommandPublicationRecord>));

			composite.Configure(compositeAppSettings);

			composite.AddAgent(typeof(TestCommand).Assembly);
			composite.AddAgent(typeof(LogQueries).Assembly);

			composite.RegisterInputModelMap<TestInputModel, TestCommand>(); // (new TestInputModelToCommandConverter());
			composite.RegisterInputModelMap<FailingInputModel, FailingCommand>(); // (new FailingInputModelToCommandConverter());
			composite.RegisterInputModelMap<ComplexInputModel, ComplexCommand>(
				i => new ComplexCommand { StringValue = i.StringValue, StringLength = i.StringValue.Length });
			setAzureCredentials(container);

			composite.RegisterNh(compositeDatabaseConnection, true);

			// composite.CreateSchema(compositeDatabaseConnection);

			_initialized = true;
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