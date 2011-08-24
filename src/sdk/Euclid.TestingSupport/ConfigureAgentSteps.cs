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
using log4net.Config;
using Microsoft.WindowsAzure;
using TechTalk.SpecFlow;

namespace Euclid.TestingSupport
{
	/// <summary>
	/// 	Implement a single ConfigureAgentSteps subclass per agent being tested by Specflow.
	/// </summary>
	/// <typeparam name = "TTypeFromAgent"></typeparam>
	public abstract class ConfigureAgentSteps<TTypeFromAgent>
	{
		protected WindsorContainer Container;

		private bool _configured;

		public ConsoleFabric Fabric { get; set; }

		[Given(@"the agent (.*)")]
		public void GivenTheAgent(string assemblyName)
		{
			if (!_configured)
			{
				configure(typeof(TTypeFromAgent).Assembly);
			}
		}

		[When(@"the command is complete")]
		public void WhenTheCommandIsComplete()
		{
			var attempts = 0;
			while (true & attempts < 8)
			{
				var registry = Container.Resolve<ICommandRegistry>();

				var record = registry.GetPublicationRecord(DefaultSpecSteps.PubIdOfLastMessage);

				if (record.Completed || record.Error)
				{
					break;
				}

				attempts++;
				Thread.Sleep(250);
			}
		}

		private void configure(Assembly agentAssembly)
		{
			XmlConfigurator.Configure();

			Container = new WindsorContainer();

			setAzureCredentials(Container);

			Fabric = new ConsoleFabric(Container);

			var composite = new BasicCompositeApp(Container);

			composite.RegisterNh(
				MsSqlConfiguration.MsSql2008.ConnectionString(c => c.FromConnectionStringWithKey("test-db")), true, false);

			composite.AddAgent(agentAssembly);

			composite.Configure(getCompositeSettings());

			Fabric.Initialize(getFabricSettings());

			Fabric.InstallComposite(composite);

			Fabric.Start();

			_configured = true;

			Container.Register(Component.For<BasicFabric>().Instance(Fabric));
			Container.Register(Component.For<BasicCompositeApp>().Instance(composite));

			DefaultSpecSteps.SetContainerInScenarioContext(Container);
		}

		private CompositeAppSettings getCompositeSettings()
		{
			var compositeAppSettings = new CompositeAppSettings();

			compositeAppSettings.OutputChannel.WithDefault(typeof (AzureMessageChannel));
			compositeAppSettings.BlobStorage.WithDefault(typeof(AzureBlobStorage));
			compositeAppSettings.CommandPublicationRecordMapper.WithDefault(typeof(NhRecordMapper<CommandPublicationRecord>));

			return compositeAppSettings;
		}

		private FabricRuntimeSettings getFabricSettings()
		{
			var fabricSettings = new FabricRuntimeSettings();

			fabricSettings.ServiceHost.WithDefault(typeof(MultitaskingServiceHost));
			fabricSettings.HostedServices.WithDefault(new List<Type> { typeof(CommandHost) });

            var messageChannel = new AzureMessageChannel(new JsonMessageSerializer());

            fabricSettings.InputChannel.WithDefault(messageChannel);
            fabricSettings.ErrorChannel.WithDefault(messageChannel);

			return fabricSettings;
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