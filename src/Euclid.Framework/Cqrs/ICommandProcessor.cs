namespace Euclid.Framework.Cqrs
{
	/// <summary>
	/// 	A command processor interprets an ICommand into a series of actions in the system.
	/// </summary>
	public interface ICommandProcessor<in TCommand>
		where TCommand : ICommand
	{
		void Process(TCommand command);
	}
}