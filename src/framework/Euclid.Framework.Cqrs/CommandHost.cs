﻿using System.Collections.Generic;
using Euclid.Common.Logging;
using Euclid.Common.ServiceHost;

namespace Euclid.Framework.Cqrs
{
	public class CommandHost : DefaultHostedService, ILoggingSource
	{
		private readonly IList<ICommandDispatcher> _dispatchers;

		public CommandHost(IEnumerable<ICommandDispatcher> dispatchers)
		{
			State = HostedServiceState.Unspecified;

			_dispatchers = new List<ICommandDispatcher>(dispatchers);
		}

		public void AddDispatcher(ICommandDispatcher dispatcher)
		{
			_dispatchers.Add(dispatcher);
		}

		protected override void OnStart()
		{
			foreach (var d in _dispatchers)
			{
				d.Enable();
			}
		}

		protected override void OnStop()
		{
			foreach (var d in _dispatchers)
			{
				d.Disable();
			}
		}
	}
}