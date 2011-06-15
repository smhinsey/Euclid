using System;
using System.Collections.Generic;
using System.Threading;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Euclid.Common.HostingFabric;
using Euclid.Common.ServiceHost;
using Euclid.Common.TestingFakes.ServiceHost;
using NUnit.Framework;

namespace Euclid.Common.UnitTests.HostingFabric
{
	public class LocalMachineFabricTests
	{
		[Test]
		public void ErrorFreeWithValidConfig()
		{
			var container = new WindsorContainer();

			container.Register(
			                   Component.For<IServiceHost>()
			                   	.Forward<MultitaskingServiceHost>()
			                   	.Instance(new MultitaskingServiceHost())
				);

			container.Register(
			                   Component.For<IHostedService>()
			                   	.Forward<FakeHostedService>()
			                   	.Instance(new FakeHostedService())
				);

			var runtime = new LocalMachineFabric(container);

			var settings = new FabricRuntimeSettings();

			settings.ServiceHost.WithDefault(typeof (MultitaskingServiceHost));
			settings.HostedServices.WithDefault(new List<Type> {typeof (FakeHostedService)});

			runtime.Configure(settings);
		}

		[Test]
		public void ReportsErrorsThrownByHostedServices()
		{
			var container = new WindsorContainer();

			container.Register(
												 Component.For<IServiceHost>()
													.Forward<MultitaskingServiceHost>()
													.Instance(new MultitaskingServiceHost())
				);

			container.Register(
												 Component.For<IHostedService>()
													.Forward<FailingHostedService>()
													.Instance(new FailingHostedService())
				);

			var runtime = new LocalMachineFabric(container);

			var settings = new FabricRuntimeSettings();

			settings.ServiceHost.WithDefault(typeof(MultitaskingServiceHost));
			settings.HostedServices.WithDefault(new List<Type> { typeof(FailingHostedService) });

			runtime.Configure(settings);

			runtime.Start();

			Assert.AreEqual(FabricRuntimeState.Started, runtime.State);

			Thread.Sleep(100);

			var exceptions = runtime.GetExceptionsThrownByHostedServices();

			Assert.AreEqual(1, exceptions.Count);
		}

		[Test]
		public void ReportsRuntimeStatistics()
		{
			Assert.Ignore("Down the road.");
		}

		[Test]
		public void Starts()
		{
			var container = new WindsorContainer();

			container.Register(
			                   Component.For<IServiceHost>()
			                   	.Forward<MultitaskingServiceHost>()
			                   	.Instance(new MultitaskingServiceHost())
				);

			container.Register(
			                   Component.For<IHostedService>()
			                   	.Forward<FakeHostedService>()
			                   	.Instance(new FakeHostedService())
				);

			var runtime = new LocalMachineFabric(container);

			var settings = new FabricRuntimeSettings();

			settings.ServiceHost.WithDefault(typeof (MultitaskingServiceHost));
			settings.HostedServices.WithDefault(new List<Type> {typeof (FakeHostedService)});

			runtime.Configure(settings);

			runtime.Start();

			Assert.AreEqual(FabricRuntimeState.Started, runtime.State);
		}

		[Test]
		public void StartsAndRestarts()
		{
			var container = new WindsorContainer();

			container.Register(
			                   Component.For<IServiceHost>()
			                   	.Forward<MultitaskingServiceHost>()
			                   	.Instance(new MultitaskingServiceHost())
				);

			container.Register(
			                   Component.For<IHostedService>()
			                   	.Forward<FakeHostedService>()
			                   	.Instance(new FakeHostedService())
				);

			var runtime = new LocalMachineFabric(container);

			var settings = new FabricRuntimeSettings();

			settings.ServiceHost.WithDefault(typeof (MultitaskingServiceHost));
			settings.HostedServices.WithDefault(new List<Type> {typeof (FakeHostedService)});

			runtime.Configure(settings);

			runtime.Start();

			Assert.AreEqual(FabricRuntimeState.Started, runtime.State);

			runtime.Shutdown();

			Assert.AreEqual(FabricRuntimeState.Stopped, runtime.State);

			runtime.Start();

			Assert.AreEqual(FabricRuntimeState.Started, runtime.State);
		}

		[Test]
		public void StartsAndStops()
		{
			var container = new WindsorContainer();

			container.Register(
			                   Component.For<IServiceHost>()
			                   	.Forward<MultitaskingServiceHost>()
			                   	.Instance(new MultitaskingServiceHost())
				);

			container.Register(
			                   Component.For<IHostedService>()
			                   	.Forward<FakeHostedService>()
			                   	.Instance(new FakeHostedService())
				);

			var runtime = new LocalMachineFabric(container);

			var settings = new FabricRuntimeSettings();

			settings.ServiceHost.WithDefault(typeof (MultitaskingServiceHost));
			settings.HostedServices.WithDefault(new List<Type> {typeof (FakeHostedService)});

			runtime.Configure(settings);

			runtime.Start();

			Assert.AreEqual(FabricRuntimeState.Started, runtime.State);

			runtime.Shutdown();

			Assert.AreEqual(FabricRuntimeState.Stopped, runtime.State);
		}

		[Test]
		[ExpectedException(typeof (NoServiceHostConfiguredException))]
		public void ThrowsWhenConfigIsEmpty()
		{
			var container = new WindsorContainer();

			var runtime = new LocalMachineFabric(container);

			runtime.Configure(new FabricRuntimeSettings());
		}

		[Test]
		[ExpectedException(typeof (NoHostedServicesConfiguredException))]
		public void ThrowsWhenConfigIsMissingHostedServices()
		{
			var container = new WindsorContainer();

			var runtime = new LocalMachineFabric(container);

			var settings = new FabricRuntimeSettings();

			settings.ServiceHost.WithDefault(GetType());

			runtime.Configure(settings);
		}

		[Test]
		[ExpectedException(typeof (NoServiceHostConfiguredException))]
		public void ThrowsWhenConfigIsMissingServiceHost()
		{
			var container = new WindsorContainer();

			var runtime = new LocalMachineFabric(container);

			var settings = new FabricRuntimeSettings();

			settings.HostedServices.WithDefault(new List<Type>());

			runtime.Configure(settings);
		}

		[Test]
		[ExpectedException(typeof (HostedServiceNotResolvableException))]
		public void ThrowsWhenHostedServiceNotResolvable()
		{
			var container = new WindsorContainer();

			container.Register(
			                   Component.For<IServiceHost>()
			                   	.Forward<MultitaskingServiceHost>()
			                   	.Instance(new MultitaskingServiceHost())
				);

			var runtime = new LocalMachineFabric(container);

			var settings = new FabricRuntimeSettings();

			settings.ServiceHost.WithDefault(typeof (MultitaskingServiceHost));
			settings.HostedServices.WithDefault(new List<Type> {GetType()});

			runtime.Configure(settings);
		}

		[Test]
		[ExpectedException(typeof (ServiceHostNotResolvableException))]
		public void ThrowsWhenServiceHostNotResolvable()
		{
			var container = new WindsorContainer();

			var runtime = new LocalMachineFabric(container);

			var settings = new FabricRuntimeSettings();

			settings.ServiceHost.WithDefault(GetType());
			settings.HostedServices.WithDefault(new List<Type> {GetType()});

			runtime.Configure(settings);
		}
	}
}