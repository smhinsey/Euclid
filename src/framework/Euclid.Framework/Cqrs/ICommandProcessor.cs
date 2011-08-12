using Euclid.Common.Messaging;
using Euclid.Framework.Agent.Metadata;

namespace Euclid.Framework.Cqrs
{
	/// <summary>
	/// 	A command processor interprets an ICommand into a series of actions in the system.
	/// </summary>
	public interface ICommandProcessor<in TCommand> : IMessageProcessor<TCommand>, IAgentPart, ICommandProcessor
		where TCommand : ICommand
	{
	}

	public interface ICommandProcessor
	{
	}
}