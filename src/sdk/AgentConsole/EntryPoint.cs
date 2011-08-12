using System;
using System.Collections.Generic;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Euclid.Common.Messaging;
using Euclid.Common.ServiceHost;
using Euclid.Common.Storage.Azure;
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
			BasicConfigurator.Configure();

			var container = new WindsorContainer();

			setAzureCredentials(container);

			var fabric = new ConsoleFabric(container);

			var composite = new BasicCompositeApp(container);

			try
			{
				composite.RegisterNh(SQLiteConfiguration.Standard.UsingFile("AgentConsoleDb"), true, false);

				composite.AddAgent(typeof (CommentOnPost).Assembly);

				composite.Configure(getCompositeSettings());

				fabric.Initialize(getFabricSettings());

				fabric.InstallComposite(composite);

				fabric.Start();

				var publisher = container.Resolve<IPublisher>();

				publisher.PublishMessage(new CommentOnPost());

				Console.ReadLine();
			}
			catch (Exception e)
			{
				fabric.ShowError(e);
				Console.ReadLine();
			}
		}

		private static CompositeAppSettings getCompositeSettings()
		{
			var compositeAppSettings = new CompositeAppSettings();

			compositeAppSettings.BlobStorage.WithDefault(typeof (AzureBlobStorage));

			return compositeAppSettings;
		}

		private static FabricRuntimeSettings getFabricSettings()
		{
			var fabricSettings = new FabricRuntimeSettings();

			fabricSettings.ServiceHost.WithDefault(typeof (MultitaskingServiceHost));
			fabricSettings.HostedServices.WithDefault(new List<Type> {typeof (CommandHost)});

			fabricSettings.InputChannel.WithDefault(new InMemoryMessageChannel());
			fabricSettings.ErrorChannel.WithDefault(new InMemoryMessageChannel());

			return fabricSettings;
		}

		private static void setAzureCredentials(IWindsorContainer container)
		{
			var storageAccount = new CloudStorageAccount(CloudStorageAccount.DevelopmentStorageAccount.Credentials,
			                                             CloudStorageAccount.DevelopmentStorageAccount.BlobEndpoint,
			                                             CloudStorageAccount.DevelopmentStorageAccount.QueueEndpoint,
			                                             CloudStorageAccount.DevelopmentStorageAccount.TableEndpoint);

			container.Register(Component.For<CloudStorageAccount>().Instance(storageAccount));
		}
	}
}