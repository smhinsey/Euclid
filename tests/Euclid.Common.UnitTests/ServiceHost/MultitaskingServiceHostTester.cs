using System;
using System.Threading;
using Euclid.Common.ServiceHost;
using Euclid.Common.TestingFakes.ServiceHost;
using NUnit.Framework;

namespace Euclid.Common.UnitTests.ServiceHost
{
	public class MultitaskingServiceHostTester
	{
		[Test]
		[ExpectedException(typeof (HostedServiceNotFoundException))]
		public void GetStateFailsForMissingService()
		{
			var host = new MultitaskingServiceHost();

			host.GetState(Guid.NewGuid());
		}

		[Test]
		public void InstallsStartsAndStops()
		{
			var host = new MultitaskingServiceHost();

			host.Install(new FakeHostedService());
			host.Install(new FakeHostedService());

			host.StartAll();

			Thread.Sleep(100);

			Assert.AreEqual(ServiceHostState.Started, host.State);

			host.StopAll();

			Assert.AreEqual(ServiceHostState.Stopped, host.State);
		}

		[Test]
		public void ModifyIndividualServiceState()
		{
			var host = new MultitaskingServiceHost();

			var serviceId = host.Install(new FakeHostedService());

			host.StartAll();

			Assert.AreEqual(ServiceHostState.Started, host.State);
			Assert.AreEqual(HostedServiceState.Started, host.GetState(serviceId));

			Thread.Sleep(100);

			host.StopAll();

			Assert.AreEqual(ServiceHostState.Stopped, host.State);
			Assert.AreEqual(HostedServiceState.Stopped, host.GetState(serviceId));
		}

		[Test]
		public void ScaleAllUp()
		{
			const int initialScale = 5;

			var host = new MultitaskingServiceHost(initialScale);

			host.Install(new FakeHostedService());

			host.StartAll();

			Assert.AreEqual(ServiceHostState.Started, host.State);
			Assert.AreEqual(initialScale, host.Scale);

			host.ScaleAllUp();

			Assert.AreEqual(initialScale + 1, host.Scale);
		}

		[Test]
		public void ScaleAllDown()
		{
			const int initialScale = 5;

			var host = new MultitaskingServiceHost(initialScale);

			host.Install(new FakeHostedService());

			host.StartAll();

			Assert.AreEqual(ServiceHostState.Started, host.State);
			Assert.AreEqual(initialScale, host.Scale);

			host.ScaleAllDown();

			Assert.AreEqual(initialScale - 1, host.Scale);
		}

		[Test]
		public void ScaleDown()
		{
			const int initialScale = 5;

			var host = new MultitaskingServiceHost(initialScale);

			var serviceId = host.Install(new FakeHostedService());

			host.StartAll();

			Assert.AreEqual(ServiceHostState.Started, host.State);
			Assert.AreEqual(initialScale, host.Scale);

			host.ScaleDown(serviceId);

			Assert.AreEqual(initialScale - 1, host.Scale);
		}

		[Test]
		[ExpectedException(typeof (HostedServiceNotFoundException))]
		public void ScaleDownFailsForMissingService()
		{
			var host = new MultitaskingServiceHost();

			host.ScaleDown(Guid.NewGuid());
		}

		[Test]
		public void ScaleUp()
		{
			const int initialScale = 5;

			var host = new MultitaskingServiceHost(initialScale);

			var serviceId = host.Install(new FakeHostedService());

			host.StartAll();

			Assert.AreEqual(ServiceHostState.Started, host.State);
			Assert.AreEqual(initialScale, host.Scale);

			host.ScaleUp(serviceId);

			Assert.AreEqual(initialScale + 1, host.Scale);
		}

		[Test]
		[ExpectedException(typeof (HostedServiceNotFoundException))]
		public void ScaleUpFailsForMissingService()
		{
			var host = new MultitaskingServiceHost();

			host.ScaleUp(Guid.NewGuid());
		}

		[Test]
		public void StartAndStopIndividualService()
		{
			var host = new MultitaskingServiceHost();

			var serviceId = host.Install(new FakeHostedService());

			host.Start(serviceId);

			Assert.AreEqual(ServiceHostState.Started, host.State);
			Assert.AreEqual(HostedServiceState.Started, host.GetState(serviceId));

			Thread.Sleep(100);

			host.Stop(serviceId);

			Assert.AreEqual(ServiceHostState.Stopped, host.State);
			Assert.AreEqual(HostedServiceState.Stopped, host.GetState(serviceId));
		}

		[Test]
		[ExpectedException(typeof (HostedServiceNotFoundException))]
		public void StartFailsForMissingService()
		{
			var host = new MultitaskingServiceHost();

			host.Start(Guid.NewGuid());
		}

		[Test]
		public void StartsAndStops()
		{
			var host = new MultitaskingServiceHost();

			host.StartAll();

			Assert.AreEqual(ServiceHostState.Started, host.State);

			host.StopAll();

			Assert.AreEqual(ServiceHostState.Stopped, host.State);
		}

		[Test]
		public void StartsWithCorrectScale()
		{
			const int initialScale = 5;

			var host = new MultitaskingServiceHost(initialScale);

			host.StartAll();

			Assert.AreEqual(ServiceHostState.Started, host.State);
			Assert.AreEqual(initialScale, host.Scale);
		}

		[Test]
		public void StartsWithoutError()
		{
			var host = new MultitaskingServiceHost();

			host.StartAll();

			Assert.AreEqual(ServiceHostState.Started, host.State);
		}

		[Test]
		[ExpectedException(typeof (HostedServiceNotFoundException))]
		public void StopFailsForMissingService()
		{
			var host = new MultitaskingServiceHost();

			host.Stop(Guid.NewGuid());
		}
	}
}