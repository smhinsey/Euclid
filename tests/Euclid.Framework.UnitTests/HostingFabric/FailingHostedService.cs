using System;
using Euclid.Common.ServiceHost;

namespace Euclid.Framework.UnitTests.HostingFabric
{
	public class FailingHostedService : DefaultHostedService
	{
		protected override void OnStart()
		{
			throw new Exception("Ha!");
		}

		protected override void OnStop()
		{
		}
	}
}
