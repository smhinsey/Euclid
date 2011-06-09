using System.Threading;
using Euclid.Common.ServiceHost;
using Euclid.Common.TestingFakes.ServiceHost;
using NUnit.Framework;

namespace Euclid.Common.UnitTests.ServiceHost
{
	public class MultitaskingServiceHostTester
	{
		[Test]
		public void ScaleDown()
		{
			const int initialScale = 5;

			var host = new MultitaskingServiceHost(initialScale);

			host.StartAll();

			Assert.AreEqual(ServiceHostState.Started, host.State);
			Assert.AreEqual(initialScale, host.Scale);

			host.ScaleAllDown();

			Assert.AreEqual(initialScale - 1, host.Scale);
		}

		[Test]
		public void ScaleUp()
		{
			const int initialScale = 5;

			var host = new MultitaskingServiceHost(initialScale);

			host.StartAll();

			Assert.AreEqual(ServiceHostState.Started, host.State);
			Assert.AreEqual(initialScale, host.Scale);

			host.ScaleAllUp();

			Assert.AreEqual(initialScale + 1, host.Scale);
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
	}
}