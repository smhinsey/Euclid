using System.Collections.Generic;
using Euclid.Common.Logging;
using Euclid.Common.Messaging;
using Euclid.Common.ServiceHost;
using Euclid.Common.Storage;
using Microsoft.Practices.ServiceLocation;

namespace Euclid.Framework.Cqrs
{
    public class CommandHost : DefaultHostedService, ILoggingSource
    {
        private readonly IEnumerable<ICommandDispatcher> _dispatchers;

        public CommandHost(IEnumerable<ICommandDispatcher> dispatchers)
        {
            State = HostedServiceState.Unspecified;

            _dispatchers = new List<ICommandDispatcher>(dispatchers);
        }

        protected override void OnStart()
        {
            foreach(var d in _dispatchers)
            {
                d.Enable();
            }
        }

        protected override void OnStop()
        {
            foreach(var d in _dispatchers)
            {
                d.Disable();
            }
        }
    }
}
