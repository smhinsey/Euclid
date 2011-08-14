using System;

namespace Euclid.Common.Messaging
{
	public interface IPublicationRegistry<out TRecord>
		where TRecord : class, IPublicationRecord
	{
		TRecord CreateRecord(IMessage message);
		IMessage GetMessage(Uri messageLocation, Type messageType);
		TRecord GetRecord(Guid identifier);

		TRecord MarkAsComplete(Guid id);
		TRecord MarkAsFailed(Guid id, string message = null, string callStack = null);
		TRecord MarkAsUnableToDispatch(Guid id, bool isError = false, string message = null);
	}
}