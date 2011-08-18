using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using AgentConsole;
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
using Microsoft.WindowsAzure;
using TechTalk.SpecFlow;
using log4net.Config;

namespace Euclid.TestingSupport
{
	public class ConfigureAgentSteps<TTypeFromAgent>
	{
		protected WindsorContainer Container;
		private bool _configured;
		private ConsoleFabric _fabric;

		[Given(@"a runtime fabric for agent (.*)")]
		public void GivenARuntimeFabricForAgent(string assemblyName)
		{
			if (!_configured)
			{
				configure(typeof (TTypeFromAgent).Assembly);
			}
		}

		[When(@"the command is complete")]
		public void WhenTheCommandIsComplete()
		{
			while (true)
			{
				var registry = Container.Resolve<ICommandRegistry>();

				var record = registry.GetRecord(DefaultSpecSteps.PubIdOfLastMessage);

				if (record.Completed || record.Error)
				{
					break;
				}

				Thread.Sleep(250);
			}
		}

		private void configure(Assembly agentAssembly)
		{
			XmlConfigurator.Configure();

			Container = new WindsorContainer();

			setAzureCredentials(Container);

			_fabric = new ConsoleFabric(Container);

			var composite = new BasicCompositeApp(Container);

			composite.RegisterNh(MsSqlConfiguration.MsSql2008.ConnectionString(c => c.FromConnectionStringWithKey("test-db")), true, false);

			composite.AddAgent(agentAssembly);

			composite.Configure(getCompositeSettings());

			_fabric.Initialize(getFabricSettings());

			_fabric.InstallComposite(composite);

			_fabric.Start();

			_configured = true;

			DefaultSpecSteps.SetContainerInScenarioContext(Container);
		}

		private CompositeAppSettings getCompositeSettings()
		{
			var compositeAppSettings = new CompositeAppSettings();

			compositeAppSettings.MessageChannel.WithDefault(typeof (AzureMessageChannel));
			compositeAppSettings.BlobStorage.WithDefault(typeof (AzureBlobStorage));
			compositeAppSettings.CommandPublicationRecordMapper.WithDefault(typeof (NhRecordMapper<CommandPublicationRecord>));

			return compositeAppSettings;
		}

		private FabricRuntimeSettings getFabricSettings()
		{
			var fabricSettings = new FabricRuntimeSettings();

			fabricSettings.ServiceHost.WithDefault(typeof (MultitaskingServiceHost));
			fabricSettings.HostedServices.WithDefault(new List<Type> {typeof (CommandHost)});

			var messageChannel = new AzureMessageChannel(new JsonMessageSerializer());

			fabricSettings.InputChannel.WithDefault(messageChannel);
			fabricSettings.ErrorChannel.WithDefault(messageChannel);

			return fabricSettings;
		}

		private void setAzureCredentials(IWindsorContainer container)
		{
			// as soon as we can stop using the azure storage emulator we should

			var storageAccount = new CloudStorageAccount(CloudStorageAccount.DevelopmentStorageAccount.Credentials,
			                                             CloudStorageAccount.DevelopmentStorageAccount.BlobEndpoint,
			                                             CloudStorageAccount.DevelopmentStorageAccount.QueueEndpoint,
			                                             CloudStorageAccount.DevelopmentStorageAccount.TableEndpoint);

			container.Register(Component.For<CloudStorageAccount>().Instance(storageAccount));
		}
	}
}