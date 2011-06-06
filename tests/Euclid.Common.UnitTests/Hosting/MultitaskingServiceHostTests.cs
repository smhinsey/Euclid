using Euclid.Common.Hosting;
using Euclid.Common.ServiceHost;
using NUnit.Framework;

namespace Euclid.Common.UnitTests.Hosting
{
	public class MultitaskingServiceHostTests
	{
		[Test]
		public void ScaleDown()
		{
			Assert.Fail();
		}

		[Test]
		public void ScaleUp()
		{
			Assert.Fail();
		}

		[Test]
		public void StartsWithoutError()
		{
			var host = new MultitaskingServiceHost();

			host.StartAll();

			Assert.AreEqual(ServiceHostState.Started, host.State);
		}

		[Test]
		public void StartsAndStops()
		{
			Assert.Fail();
		}
	}
}