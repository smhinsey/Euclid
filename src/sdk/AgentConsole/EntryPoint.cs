using System;
using System.Collections.Generic;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
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
using Microsoft.WindowsAzure;
using log4net.Config;

namespace AgentConsole
{
	public class EntryPoint
	{
		public static void Main(string[] args)
		{
			XmlConfigurator.Configure();

			var container = new WindsorContainer();

			setAzureCredentials(container);

			var fabric = new ConsoleFabric(container);

			var composite = new BasicCompositeApp(container)
				{ Name = "AgentConsole Composite", Description = "The composite app used by the agent console" };

			try
			{
				composite.RegisterNh(
					MsSqlConfiguration.MsSql2008.ConnectionString(c => c.FromConnectionStringWithKey("test-db")), false, false);

				composite.AddAgent(typeof(PublishPost).Assembly);

				composite.Configure(getCompositeSettings());

				fabric.Initialize(getFabricSettings());

				fabric.InstallComposite(composite);

				fabric.Start();

				Console.WriteLine("Press enter to exit console");

				Console.ReadLine();
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				Console.ReadLine();
			}
		}

		private static CompositeAppSettings getCompositeSettings()
		{
			var compositeAppSettings = new CompositeAppSettings();

			compositeAppSettings.BlobStorage.WithDefault(typeof(AzureBlobStorage));

			compositeAppSettings.OutputChannel.WithDefault(typeof(AzureMessageChannel));

			compositeAppSettings.CommandPublicationRecordMapper.WithDefault(typeof(NhRecordMapper<CommandPublicationRecord>));

			return compositeAppSettings;
		}

		private static FabricRuntimeSettings getFabricSettings()
		{
			var fabricSettings = new FabricRuntimeSettings();

			fabricSettings.ServiceHost.WithDefault(typeof(MultitaskingServiceHost));
			fabricSettings.HostedServices.WithDefault(new List<Type> { typeof(CommandHost) });

			var messageChannel = new AzureMessageChannel(new JsonMessageSerializer());

			messageChannel.Open();

			messageChannel.Clear();

			messageChannel.Close();

			fabricSettings.InputChannel.WithDefault(messageChannel);
			fabricSettings.ErrorChannel.WithDefault(messageChannel);

			return fabricSettings;
		}

		private static void setAzureCredentials(IWindsorContainer container)
		{
			var storageAccount = new CloudStorageAccount(
				CloudStorageAccount.DevelopmentStorageAccount.Credentials,
				CloudStorageAccount.DevelopmentStorageAccount.BlobEndpoint,
				CloudStorageAccount.DevelopmentStorageAccount.QueueEndpoint,
				CloudStorageAccount.DevelopmentStorageAccount.TableEndpoint);

			container.Register(Component.For<CloudStorageAccount>().Instance(storageAccount));
		}
	}
}