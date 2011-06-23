using Euclid.Common.Logging;
using Euclid.Common.Messaging;
using Euclid.Common.ServiceHost;
using Euclid.Common.Storage;
using Microsoft.Practices.ServiceLocation;

namespace Euclid.Framework.Cqrs
{
    public class CommandHost : DefaultHostedService, ILoggingSource
    {
        private readonly ICommandDispatcher _dispatcher;

        public CommandHost(IServiceLocator config, IMessageDispatcherSettings dispatcherSettings)
        {
            State = HostedServiceState.Unspecified; 
            var repo = config.GetInstance<IBasicRecordRepository<CommandPublicationRecord>>();
            var blob = config.GetInstance<IBlobStorage>();
            var serializer = config.GetInstance<IMessageSerializer>();

            var registry = new CommandRegistry(repo, blob, serializer);

            //TODO: instantiating the command dispatcher may be limiting
            _dispatcher = new CommandDispatcher(config, registry);
            _dispatcher.Configure(dispatcherSettings);
        }

        protected override void OnStart()
        {
            _dispatcher.Enable();
            this.WriteInfoMessage("Dispatcher enabled [{0}]", _dispatcher.GetType().FullName);
        }

        protected override void OnStop()
        {
            _dispatcher.Disable();
            this.WriteInfoMessage("Dispatcher disabled [{0}]", _dispatcher.GetType().FullName);
        }
    }
}
