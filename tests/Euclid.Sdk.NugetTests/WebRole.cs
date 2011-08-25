using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Euclid.Common.Messaging.Azure;
using Euclid.Common.Storage.Azure;
using Euclid.Common.Storage.NHibernate;
using Euclid.Composites;
using Euclid.Composites.Mvc;
using Euclid.Framework.Cqrs;
using Euclid.Sdk.TestAgent.Commands;
using FluentNHibernate.Cfg.Db;
using Microsoft.WindowsAzure;
using MvcContrib.PortableAreas;
using MvcContrib.UI.InputBuilder;

namespace Euclid.Sdk.NugetTests
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

            var container = new WindsorContainer();

			var composite = new MvcCompositeApp(container);

			composite.RegisterNh(
				MsSqlConfiguration.MsSql2008.ConnectionString(c => c.FromConnectionStringWithKey("test-db")), true, false);

			var compositeAppSettings = new CompositeAppSettings();

            compositeAppSettings.OutputChannel.ApplyOverride(typeof(AzureMessageChannel));
			compositeAppSettings.BlobStorage.WithDefault(typeof(AzureBlobStorage));
			compositeAppSettings.CommandPublicationRecordMapper.WithDefault(typeof(NhRecordMapper<CommandPublicationRecord>));

			composite.Configure(compositeAppSettings);

			/* EUCLID: Install agents and Input models */
            composite.AddAgent(typeof(TestCommand).Assembly);

			container.Register(Component.For<ICompositeApp>().Instance(composite));

			setAzureCredentials(container);

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