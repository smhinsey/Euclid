using Euclid.Common.Messaging;

namespace Euclid.Framework.Cqrs
{
	/// <summary>
	/// 	A command processor interprets an ICommand into a series of actions in the system.
	/// </summary>
	public interface ICommandProcessor<in TCommand> : IMessageProcessor<TCommand>
		where TCommand : ICommand
	{
	}
}