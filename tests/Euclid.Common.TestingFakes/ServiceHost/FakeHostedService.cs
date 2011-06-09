using System.Threading;
using Euclid.Common.ServiceHost;

namespace Euclid.Common.TestingFakes.ServiceHost
{
	public class FakeHostedService : DefaultHostedService
	{
		public FakeHostedService() : base(typeof (FakeHostedService).Name)
		{
		}

		public override void Pause()
		{
			State = HostedServiceState.Paused;
		}

		public override void Start()
		{
			State = HostedServiceState.Started;

			while (true)
			{
				Thread.Sleep(100);
			}
		}

		public override void Stop()
		{
			State = HostedServiceState.Stopped;
		}
	}
}