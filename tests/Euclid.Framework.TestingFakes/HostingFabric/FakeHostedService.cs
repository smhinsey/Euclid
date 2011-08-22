using System;
using System.Threading;
using System.Threading.Tasks;
using Euclid.Common.ServiceHost;

namespace Euclid.Framework.TestingFakes.HostingFabric
{
	public class FakeHostedService : DefaultHostedService
	{
		protected override void OnStart()
		{
			Console.WriteLine(string.Format("{1}({0})[{2}]: started", Task.CurrentId, this.Name, this.State));

			for (var i = 0; i < 100; i++)
			{
				Console.WriteLine("{3}({2})[{0}]: {1}", this.State, i + 1, Task.CurrentId, this.Name);
				Thread.Sleep(10);

				if (this.State == HostedServiceState.Stopped)
				{
					Console.WriteLine(string.Format("{1}({0})[{2}]: stopped", Task.CurrentId, this.Name, this.State));

					break;
				}
			}
		}

		protected override void OnStop()
		{
			Console.WriteLine(string.Format("{1}({0})[{2}]: stopping", Task.CurrentId, this.Name, this.State));
		}
	}
}