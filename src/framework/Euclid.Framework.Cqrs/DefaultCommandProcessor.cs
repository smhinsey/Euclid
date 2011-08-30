using Euclid.Common.Logging;
using Euclid.Common.Messaging;

namespace Euclid.Framework.Cqrs
{
	public abstract class DefaultCommandProcessor<TCommand> : DefaultLoggingSource, ICommandProcessor<TCommand>
		where TCommand : ICommand
	{
		public bool CanProcessMessage(IMessage message)
		{
			return message.GetType() == typeof(TCommand);
		}

		public abstract void Process(TCommand message);
	}
}