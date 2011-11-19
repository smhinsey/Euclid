using System;
using System.IO;
using AdminComposite.Models;
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
using LoggingAgent.Queries;
using Microsoft.WindowsAzure;
using NConfig;
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

			NConfigurator.UsingFile(@"~\Config\custom.config")
				.SetAsSystemDefault();

			XmlConfigurator.Configure(new FileInfo(Path.Combine(Environment.CurrentDirectory, NConfigurator.Default.FileNames[0]))); ;

			var databaseConfiguration =
				MsSqlConfiguration.MsSql2008.ConnectionString(c => c.FromConnectionStringWithKey("forum-db"));

			var container = new WindsorContainer();

			var composite = new MvcCompositeApp(container) { Name = "Admin", Description = "Create and manage custom forums." };

			composite.RegisterNh(databaseConfiguration, true);

			var compositeAppSettings = new CompositeAppSettings();

			compositeAppSettings.OutputChannel.ApplyOverride(typeof(AzureMessageChannel));
			compositeAppSettings.BlobStorage.WithDefault(typeof(AzureBlobStorage));
			compositeAppSettings.CommandPublicationRecordMapper.WithDefault(typeof(NhRecordMapper<CommandPublicationRecord>));

			composite.Configure(compositeAppSettings);

			composite.CreateSchema(databaseConfiguration, true);

			composite.AddAgent(typeof(PublishPost).Assembly);
			composite.AddAgent(typeof(LogQueries).Assembly);

			composite.RegisterInputModelMap<CreateForumInputModel, CreateForum>(
				input => new CreateForum
				         	{
								CreatedBy = input.CreatedBy,
								Description = input.Description,
								Name = input.Name,
								OrganizationId = input.OrganizationId,
								UpDownVoting = input.VotingScheme == VotingScheme.UpDownVoting,
								UrlHostName = input.UrlHostName,
								UrlSlug = input.UrlSlug,
								Moderated = input.Moderated,
								Private = input.Private,
								Theme = input.Theme
				         	}
				);
			composite.RegisterInputModelMap<CreateOrganizationAndRegisterUserInputModel, CreateOrganizationAndRegisterUser>(
				input =>
				new CreateOrganizationAndRegisterUser
					{
						Address = input.Address,
						Address2 = input.Address2,
						City = input.City,
						Country = input.Country,
						Email = input.Email,
						FirstName = input.FirstName,
						LastName = input.LastName,
						Username = input.Username,
						OrganizationName = input.OrganizationName,
						OrganizationSlug = input.OrganizationSlug,
						OrganizationUrl = input.OrganizationUrl,
						PhoneNumber = input.PhoneNumber,
						State = input.State,
						Zip = input.Zip,
						// TODO: salt & hash password
						PasswordHash = input.Password,
						PasswordSalt = input.Password
					});
			//the processor will handle generating passwords for users registered by an admin
			composite.RegisterInputModelMap<RegisterOrganizationUserInputModel, RegisterOrganizationUser>();
			composite.RegisterInputModelMap<UpdateOrganizationUserInputModel, UpdateOrganizationUser>(
				input =>
				new UpdateOrganizationUser
					{
						Created = DateTime.Now,
						Email = input.Email,
						FirstName = input.FirstName,
						LastName = input.LastName,
						OrganizationId = input.OrganizationId,
						UserId = input.UserId,
						Username = input.Username
					});
			composite.RegisterInputModelMap<UpdateOrganizationInputModel, UpdateOrganization>();
			composite.RegisterInputModelMap<UpdateForumInputModel, UpdateForum>();

			composite.RegisterInputModelMap<RegisterForumUserInputModel, RegisterForumUser>(input => new RegisterForumUser
																										{
																											ForumIdentifier = input.ForumIdentifier,
																											FirstName = input.FirstName,
																											LastName = input.LastName,
																											Email = input.Email,
																											Username = input.Username,
																											PasswordHash = input.Password,
																											PasswordSalt = input.Password,
																											CreatedBy = input.CreatedBy
																										});

			composite.RegisterInputModelMap<SetVotingSchemeInputModel, UpdateForumVotingScheme>(
				input => new UpdateForumVotingScheme
				         	{
				         		ForumIdentifier = input.ForumIdentifier,
								NoVoting = input.SelectedScheme == VotingScheme.NoVoting,
								UpDownVoting = input.SelectedScheme == VotingScheme.UpDownVoting
				         	});

			composite.RegisterInputModelMap<CreateCategoryInputModel, CreateCategory>();
			composite.RegisterInputModelMap<UpdateCategoryInputModel, UpdateCategory>(input=>new UpdateCategory
			                                                                                 	{
			                                                                                 		CategoryIdentifier = input.Identifier,
																									Active = input.Active,
																									Name = input.Name
			                                                                                 	});

			composite.RegisterInputModelMap<CreateForumContentInputModel, CreateForumContent>();
			composite.RegisterInputModelMap<UpdateForumContentInputModel, UpdateForumContent>();
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