using Euclid.Common.Messaging;
using Euclid.Framework.Cqrs;

namespace ForumTests.Steps
{
	public interface ICommandCompleteStep<in T>
		where T : ICommand
	{
		void CommandCompleted(IPublicationRecord record, T command);
	}
}