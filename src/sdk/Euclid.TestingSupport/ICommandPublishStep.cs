using Euclid.Framework.Cqrs;

namespace Euclid.TestingSupport
{
	public interface ICommandPublishStep<T>
		where T : ICommand
	{
		T GetCommand(T command);
	}
}