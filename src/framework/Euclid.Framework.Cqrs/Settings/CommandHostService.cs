using System.Collections.Generic;

namespace Euclid.Framework.Cqrs.Settings
{
	public class CommandHostService
	{
		private readonly IList<ICommandDispatcher> _dispatchers;

		private CommandHostService()
		{
			this._dispatchers = new List<ICommandDispatcher>();
		}

		public static CommandHostService Configure()
		{
			return new CommandHostService();
		}

		public CommandHostService AddDispatcher(Dispatcher dispatcher)
		{
			this._dispatchers.Add(Dispatcher.GetConfiguredCommandDispatcher(dispatcher));

			return this;
		}

		public CommandHost GetCommandHost()
		{
			return new CommandHost(this._dispatchers);
		}
	}
}