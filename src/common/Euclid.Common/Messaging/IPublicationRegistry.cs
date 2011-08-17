using System;

namespace Euclid.Common.Messaging
{
	// SELF I'm not sure what to do about TRecord. It's not used for anything, but it allows
	// an implementation to specify a concrete type for TRecordContract which can be used
	// to create new instances. Since the declaration is otherwise unused, I'm not sure about
	// this approach, but for now, it solves a problem that resulted in an ugly implementation
	// for CommandPublicationRegistry
	public interface IPublicationRegistry<out TRecord, out TRecordContract>
		where TRecord : class, TRecordContract, IPublicationRecord
		where TRecordContract : IPublicationRecord
	{
		TRecordContract CreateRecord(IMessage message);
		IMessage GetMessage(Uri messageLocation, Type messageType);
		TRecordContract GetRecord(Guid identifier);

		TRecordContract MarkAsComplete(Guid id);
		TRecordContract MarkAsFailed(Guid id, string message = null, string callStack = null);
		TRecordContract MarkAsUnableToDispatch(Guid id, bool isError = false, string message = null);
	}
}