using System.Collections.Generic;
using Euclid.Common.Logging;
using Euclid.Common.ServiceHost;

namespace Euclid.Framework.Cqrs
{
	public class CommandHost : DefaultHostedService, ILoggingSource
	{
		private readonly IEnumerable<ICommandDispatcher> _dispatchers;

		public CommandHost(IEnumerable<ICommandDispatcher> dispatchers)
		{
			this.State = HostedServiceState.Unspecified;

			this._dispatchers = new List<ICommandDispatcher>(dispatchers);
		}

		protected override void OnStart()
		{
			foreach (var d in this._dispatchers)
			{
				d.Enable();
			}
		}

		protected override void OnStop()
		{
			foreach (var d in this._dispatchers)
			{
				d.Disable();
			}
		}
	}
}