using System;

namespace Euclid.Common.Messaging
{
	public interface IPublicationRegistry<out TRecord, out TRecordContract>
		where TRecord : class, IPublicationRecord
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