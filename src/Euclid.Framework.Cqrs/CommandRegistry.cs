using System;
using Euclid.Common.Messaging;
using Euclid.Common.Storage;

namespace Euclid.Framework.Cqrs
{
	public class CommandRegistry : PublicationRegistry<CommandPublicationRecord>, ICommandRegistry
	{
		public CommandRegistry(IBasicRecordMapper<CommandPublicationRecord> mapper, IBlobStorage blobStorage, IMessageSerializer serializer)
			: base(mapper, blobStorage, serializer)
		{
		}

		public new ICommandPublicationRecord CreateRecord(IMessage message)
		{
			return base.CreateRecord(message);
		}

		public new ICommandPublicationRecord GetRecord(Guid identifier)
		{
			return base.GetRecord(identifier);
		}

		public new ICommandPublicationRecord MarkAsComplete(Guid id)
		{
			return base.MarkAsComplete(id);
		}

		public new ICommandPublicationRecord MarkAsFailed(Guid id, string message, string callStack)
		{
			return base.MarkAsFailed(id, message, callStack);
		}

		public new ICommandPublicationRecord MarkAsUnableToDispatch(Guid id, bool isError, string message)
		{
			return base.MarkAsUnableToDispatch(id, isError, message);
		}
	}
}