using System;
using System.IO;
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
using ForumComposite.Models;
using LoggingAgent.Queries;
using Microsoft.WindowsAzure;
using NConfig;
using log4net.Config;

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

			NConfigurator.UsingFile(@"~\Config\custom.config")
				.SetAsSystemDefault();

			XmlConfigurator.Configure(new FileInfo(Path.Combine(Environment.CurrentDirectory, NConfigurator.Default.FileNames[0]))); ;

			MsSqlConfiguration databaseConfiguration =
				MsSqlConfiguration.MsSql2008.ConnectionString(c => c.FromConnectionStringWithKey("forum-db"));

			var container = new WindsorContainer();

			var composite = new MvcCompositeApp(container)
			                	{Name = "NewCo Forum", Description = " A highly customizable managed forum."};

			composite.RegisterNh(databaseConfiguration, true);

			var compositeAppSettings = new CompositeAppSettings();

			compositeAppSettings.OutputChannel.ApplyOverride(typeof (AzureMessageChannel));
			compositeAppSettings.BlobStorage.WithDefault(typeof (AzureBlobStorage));
			compositeAppSettings.CommandPublicationRecordMapper.WithDefault(typeof (NhRecordMapper<CommandPublicationRecord>));

			composite.Configure(compositeAppSettings);

			composite.AddAgent(typeof (PublishPost).Assembly);
			composite.AddAgent(typeof (LogQueries).Assembly);

			composite.RegisterInputModelMap<CommentOnPostInputModel, CommentOnPost>();
			composite.RegisterInputModelMap<PublishPostInputModel, PublishPost>();
			composite.RegisterInputModelMap<RegisterForumUserInputModel, RegisterForumUser>(
				m =>
				new RegisterForumUser
					{
						ForumIdentifier = m.ForumIdentifier,
						FirstName = m.FirstName,
						LastName = m.LastName,
						PasswordHash = m.Password,
						PasswordSalt = m.Password,
						Username = m.Username,
						Email = m.Email
					});
			composite.RegisterInputModelMap<UpdateUserProfileInputModel, UpdateUserProfile>();
			composite.RegisterInputModelMap<VoteOnCommentInputModel, VoteOnComment>();
			composite.RegisterInputModelMap<VoteOnPostInputModel, VoteOnPost>();
			composite.RegisterInputModelMap<MarkPostAsFavoriteInputModel, MarkPostAsFavorite>();
			composite.RegisterInputModelMap<MarkCommentAsFavoriteInputModel, MarkCommentAsFavorite>();
			composite.RegisterInputModelMap<AddForumUserAsFriendInputModel, AddForumUserAsFriend>();
			composite.RegisterInputModelMap<RemoveForumUserFriendInputModel, RemoveForumUserFriend>();

			composite.CreateSchema(databaseConfiguration, false);

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