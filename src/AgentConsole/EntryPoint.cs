﻿using System;
using System.Collections.Generic;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using CommonServiceLocator.WindsorAdapter;
using Euclid.Common.HostingFabric;
using Euclid.Common.ServiceHost;

namespace AgentConsole
{
	public class EntryPoint
	{
		public static void Main(string[] args)
		{
			var container = new WindsorContainer();

			container.Register(Component.For<IServiceHost>()
													.Forward<MultitaskingServiceHost>()
													.Instance(new MultitaskingServiceHost()));

			var locator = new WindsorServiceLocator(container);

			var fabric = new ConsoleFabric(locator);

			var settings = new FabricRuntimeSettings();

			settings.ServiceHost.WithDefault(typeof(MultitaskingServiceHost));
			settings.HostedServices.WithDefault(new List<Type>());

			try
			{
				fabric.Configure(settings);

				fabric.Start();
			}
			catch (Exception e)
			{
				fabric.ShowError(e);
			}
		}
	}
}