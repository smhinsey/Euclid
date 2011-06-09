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

			host.Start(serviceId);

			Assert.AreEqual(ServiceHostState.Started, host.State);
			Assert.AreEqual(HostedServiceState.Started, host.GetState(serviceId));

			Thread.Sleep(100);

			host.Stop(serviceId);

			Assert.AreEqual(ServiceHostState.Stopped, host.State);
			Assert.AreEqual(HostedServiceState.Stopped, host.GetState(serviceId));
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