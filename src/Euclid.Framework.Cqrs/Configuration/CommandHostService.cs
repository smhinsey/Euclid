using System.Collections.Generic;

namespace Euclid.Framework.Cqrs.Configuration
{
    public class CommandHostService
    {
        private readonly IList<ICommandDispatcher> _dispatchers;
        private CommandHostService()
        {
            _dispatchers = new List<ICommandDispatcher>();
        }

        public static CommandHostService Configure()
        {
            return new CommandHostService();
        }

        public CommandHostService AddDispatcher<T>(Dispatcher dispatcher)
        {
            _dispatchers.Add(Dispatcher.GetConfiguredCommandDispatcher(dispatcher));

            return this;
        }

        public CommandHost GetCommandHost()
        {
            return new CommandHost(_dispatchers);
        }
    }
}
