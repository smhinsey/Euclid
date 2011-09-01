using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Euclid.Common.Messaging.Azure;
using Euclid.Common.Storage.Azure;
using Euclid.Common.Storage.NHibernate;
using Euclid.Composites;
using Euclid.Composites.Mvc;
using Euclid.Framework.Cqrs;
using FluentNHibernate.Cfg.Db;
using ForumAgent.Commands;
using ForumComposite.Converters;
using Microsoft.WindowsAzure;

namespace ForumComposite
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
				{ Name = "NewCo Forum", Description = " A website where ideas and views on issues can be exchanged." };

			composite.RegisterNh(compositeDatabaseConnection, false, true);

			var compositeAppSettings = new CompositeAppSettings();

			compositeAppSettings.OutputChannel.ApplyOverride(typeof(AzureMessageChannel));
			compositeAppSettings.BlobStorage.WithDefault(typeof(AzureBlobStorage));
			compositeAppSettings.CommandPublicationRecordMapper.WithDefault(typeof(NhRecordMapper<CommandPublicationRecord>));

			composite.Configure(compositeAppSettings);

			composite.AddAgent(typeof(PublishPost).Assembly);

			composite.RegisterInputModel(new CommentOnPostInputModelConverter());
			composite.RegisterInputModel(new PublishPostInputModelConverter());
			composite.RegisterInputModel(new RegisterUserInputModelConverter());
			composite.RegisterInputModel(new UpdateUserProfileInputModelConverter());
			composite.RegisterInputModel(new VoteOnCommentInputModelConverter());
			composite.RegisterInputModel(new VoteOnPostInputModelConverter());

			composite.CreateSchema(compositeDatabaseConnection);
		
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