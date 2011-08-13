using Euclid.Common.Messaging;

namespace Euclid.Framework.Cqrs
{
	public abstract class DefaultCommandProcessor<TCommand> : ICommandProcessor<TCommand>
		where TCommand : ICommand
	{
		public abstract void Process(TCommand message);

		public bool CanProcessMessage(IMessage message)
		{
			return message.GetType() == typeof (TCommand);
		}
	}
}