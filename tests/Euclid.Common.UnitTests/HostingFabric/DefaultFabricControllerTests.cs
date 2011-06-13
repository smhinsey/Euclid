using NUnit.Framework;

namespace Euclid.Common.UnitTests.HostingFabric
{
	public class DefaultFabricControllerTests
	{
		[Test]
		public void HostedServiceFailureIsIsolated()
		{
			Assert.Fail();
		}

		[Test]
		public void InstalledServicesVisibleInController()
		{
		}

		[Test]
		public void InstallsServiceHost()
		{
		}

		[Test]
		public void InstallsServiceHostAndStarts()
		{
		}

		[Test]
		public void ServiceHostFailureIsIsolated()
		{
			Assert.Fail();
		}

		[Test]
		public void StartsAndStops()
		{
		}
	}
}