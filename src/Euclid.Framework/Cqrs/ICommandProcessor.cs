using Euclid.Common.Messaging;
using Euclid.Framework.Metadata;

namespace Euclid.Framework.Cqrs
{
	/// <summary>
	/// 	A command processor interprets an ICommand into a series of actions in the system.
	/// </summary>
	public interface ICommandProcessor<in TCommand> : IMessageProcessor<TCommand>, IAgentPart
		where TCommand : ICommand
	{
	}
}