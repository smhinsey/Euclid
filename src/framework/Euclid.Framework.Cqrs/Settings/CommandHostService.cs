using System.Collections.Generic;

namespace Euclid.Framework.Cqrs.Settings
{
	public class CommandHostService
	{
		private readonly IList<ICommandDispatcher> _dispatchers;

		private CommandHostService()
		{
			_dispatchers = new List<ICommandDispatcher>();
		}

		public CommandHostService AddDispatcher(Dispatcher dispatcher)
		{
			_dispatchers.Add(Dispatcher.GetConfiguredCommandDispatcher(dispatcher));

			return this;
		}

		public CommandHost GetCommandHost()
		{
			return new CommandHost(_dispatchers);
		}

		public static CommandHostService Configure()
		{
			return new CommandHostService();
		}
	}
}
