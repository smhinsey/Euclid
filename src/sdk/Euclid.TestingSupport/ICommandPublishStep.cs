using Euclid.Framework.Cqrs;

namespace ForumTests.Steps
{
	public interface ICommandPublishStep<T>
		where T : ICommand
	{
		T GetCommand(T command);
	}
}