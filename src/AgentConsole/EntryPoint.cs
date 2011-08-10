using System;
using System.Collections.Generic;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using CommonServiceLocator.WindsorAdapter;
using Euclid.Common.Messaging;
using Euclid.Common.ServiceHost;
using Euclid.Composites;
using Euclid.Framework.Cqrs;
using Euclid.Framework.HostingFabric;
using Euclid.Sdk.FakeAgent.Commands;
using Microsoft.Practices.ServiceLocation;

namespace AgentConsole
{
	public class EntryPoint
	{
		public static void Main(string[] args)
		{
			var container = new WindsorContainer();

			var locator = new WindsorServiceLocator(container);

			container.Register(Component.For<IServiceHost>()
													.Forward<MultitaskingServiceHost>()
													.Instance(new MultitaskingServiceHost()));

			container.Register(Component.For<IServiceLocator>().Instance(locator));

			var fabric = new ConsoleFabric(locator);

			var fabricSettings = new FabricRuntimeSettings();

			fabricSettings.ServiceHost.WithDefault(typeof (MultitaskingServiceHost));
			fabricSettings.HostedServices.WithDefault(new List<Type> {typeof (CommandHost)});

			fabricSettings.InputChannel.WithDefault(new InMemoryMessageChannel());
			fabricSettings.ErrorChannel.WithDefault(new InMemoryMessageChannel());

			var composite = new BasicCompositeApp {Container = container};

			try
			{
				composite.AddAgent(typeof (FakeCommand).Assembly);

				composite.Configure(new CompositeAppSettings());

				fabric.Initialize(fabricSettings);

				fabric.InstallComposite(composite);

				fabric.Start();
			}
			catch (Exception e)
			{
				fabric.ShowError(e);
			}
		}
	}
}