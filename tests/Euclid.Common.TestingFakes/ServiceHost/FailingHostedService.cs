using System;
using Euclid.Common.ServiceHost;

namespace Euclid.Common.TestingFakes.ServiceHost
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