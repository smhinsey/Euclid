using System;
using System.Configuration;
using System.IO;
using System.Linq;
using AdminComposite.Areas.Forum.InputModels;
using AdminComposite.InputModels;
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
using ForumAgent;
using ForumAgent.Commands;
using LoggingAgent.Queries;
using Microsoft.Web.Administration;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using NConfig;
using log4net.Config;

namespace ForumAdminComposite
{
	public class WebRole : RoleEntryPoint, ILoggingSource
	{
		private static WebRole _instance;

		private bool _initialized;

		public WebRole()
		{
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
				XmlConfigurator.Configure(new FileInfo(Path.Combine(Environment.CurrentDirectory, NConfigurator.Default.FileNames[0]))); ;
			}
			else
			{
				XmlConfigurator.Configure();
			}

			var databaseConfiguration =
				MsSqlConfiguration.MsSql2008.ConnectionString(c => c.FromConnectionStringWithKey("forum-db"));

			var container = new WindsorContainer();

			var composite = new MvcCompositeApp(container) {Name = "Newco Forum Admin", Description = "Create and manage custom forums."};

			composite.RegisterNh(databaseConfiguration, true);

			var compositeAppSettings = new CompositeAppSettings();

			compositeAppSettings.OutputChannel.WithDefault(typeof(AzureMessageChannel));
			compositeAppSettings.BlobStorage.WithDefault(typeof (AzureBlobStorage));
			compositeAppSettings.CommandPublicationRecordMapper.WithDefault(typeof (NhRecordMapper<CommandPublicationRecord>));

			composite.Configure(compositeAppSettings);

			composite.CreateSchema(databaseConfiguration, false);

			composite.AddAgent(typeof (PublishPost).Assembly);
			composite.AddAgent(typeof (LogQueries).Assembly);

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
			                                                                                         		ForumIdentifier =
			                                                                                         			input.ForumIdentifier,
			                                                                                         		FirstName =
			                                                                                         			input.FirstName,
			                                                                                         		LastName = input.LastName,
			                                                                                         		Email = input.Email,
			                                                                                         		Username = input.Username,
			                                                                                         		PasswordHash =
			                                                                                         			input.Password,
			                                                                                         		PasswordSalt =
			                                                                                         			input.Password,
			                                                                                         		CreatedBy =
			                                                                                         			input.CreatedBy
			                                                                                         	});
			composite.RegisterInputModelMap<CreateTagInputModel, CreateTag>();
			composite.RegisterInputModelMap<CreateStopWordInputModel, CreateStopWord>();
			composite.RegisterInputModelMap<UpdateTagInputModel, UpdateTag>();
			composite.RegisterInputModelMap<CreateCategoryInputModel, CreateCategory>();
			composite.RegisterInputModelMap<UpdateCategoryInputModel, UpdateCategory>();
			composite.RegisterInputModelMap<CreateForumContentInputModel, CreateForumContent>();
			composite.RegisterInputModelMap<UpdateForumContentInputModel, UpdateForumContent>();
			composite.RegisterInputModelMap<ForumThemeInputModel, SetForumTheme>(input=>new SetForumTheme
			                                                                            	{
			                                                                            		ForumIdentifier = input.ForumIdentifier,
																								ThemeName = input.SelectedTheme
			                                                                            	});
			composite.RegisterInputModelMap<CreateBadgeInputModel, CreateBadge>();
			composite.RegisterInputModelMap<UpdateBadgeInputModel, UpdateBadge>();
			composite.RegisterInputModelMap<CreateForumAvatarInputModel, CreateAvatar>();
			composite.RegisterInputModelMap<UpdateForumAvatarInputModel, UpdateAvatar>();
			composite.RegisterInputModelMap<ActivateAvatarInputModel, ActivateAvatar>();
			composite.RegisterInputModelMap<ActivateBadgeInputModel, ActivateBadge>();
			composite.RegisterInputModelMap<ActivateCategoryInputModel, ActivateCategory>();
			composite.RegisterInputModelMap<ActivateTagInputModel, ActivateTag>();
			composite.RegisterInputModelMap<ActivateStopWordInputModel, ActivateStopWord>();
			composite.RegisterInputModelMap<ActivateContentInputModel, ActivateContent>();
			composite.RegisterInputModelMap<DeleteContentInputModel, DeleteForumContent>();
			composite.RegisterInputModelMap<DeleteStopWordInputModel, DeleteStopWord>();
			composite.RegisterInputModelMap<ActivateUserInputModel, ActivateForumUser>();
			composite.RegisterInputModelMap<BlockUserInputModel, BlockUser>();
			composite.RegisterInputModelMap<UnblockUserInputModel, UnblockUser>();
			composite.RegisterInputModelMap<UpdateForumVotingSchemeInputModel, UpdateForumVotingScheme>(
																							input => new UpdateForumVotingScheme
																							{
																								ForumIdentifier = input.ForumIdentifier,
																								NoVoting = input.SelectedScheme == VotingScheme.NoVoting,
																								UpDownVoting = input.SelectedScheme == VotingScheme.UpDownVoting
																							});
			composite.RegisterInputModelMap<ActivateOrganizationUserInputModel, ActivateOrganizationUser>();
			composite.RegisterInputModelMap<ApprovePostInputModel, ApprovePost>();
			composite.RegisterInputModelMap<RejectPostInputModel, RejectPost>();
			composite.RegisterInputModelMap<ApproveCommentInputModel, ApproveComment>(input=>new ApproveComment
			                                                                                 	{
			                                                                                 		ApprovedBy = input.ApprovedBy,
																									CommentIdentifier = input.PostIdentifier,
																									CreatedBy = input.CreatedBy
			                                                                                 	});
			composite.RegisterInputModelMap<RejectCommentInputModel, RejectComment>(input=>new RejectComment
			                                                                               	{
			                                                                               		CommentIdentifier = input.PostIdentifier,
																								CreatedBy = input.CreatedBy
			                                                                               	});
			composite.RegisterInputModelMap<DeleteOrganizationUserInputModel, DeleteOrganizationUser>();
			composite.RegisterInputModelMap<DeleteForumUserInputModel, DeleteForumUser>();
			composite.RegisterInputModelMap<DeleteAvatarInputModel, DeleteAvatar>();

			setAzureCredentials(container);
			_initialized = true;
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