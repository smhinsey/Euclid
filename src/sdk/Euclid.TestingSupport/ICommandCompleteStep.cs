using Euclid.Common.Messaging;
using Euclid.Framework.Cqrs;

namespace Euclid.TestingSupport
{
	public interface ICommandCompleteStep<in T>
		where T : ICommand
	{
		void CommandCompleted(IPublicationRecord record, T command);
	}
}