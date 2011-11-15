﻿using System;
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
using ForumAgent.Queries;
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

			composite.RegisterInputModelMap<CreateForumInputModel, CreateForum>();
			composite.RegisterInputModelMap<CreateOrganizationAndUserInputModel, CreateOrganizationAndRegisterUser>(
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

			setAzureCredentials(container);

			container.Register(
				Component.For<OrganizationUserQueries>().ImplementedBy<OrganizationUserQueries>().LifeStyle.PerWebRequest);

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