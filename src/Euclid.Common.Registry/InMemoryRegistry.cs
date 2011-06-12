using System;
using Euclid.Common.Storage;
using Euclid.Common.Transport;

namespace Euclid.Common.Registry
{
	public class InMemoryRegistry<TRecord, TMessage> : IRegistry<TRecord, TMessage>
		where TRecord : IRecord<TMessage>, new()
		where TMessage : IMessage
	{
		private readonly IBasicRecordRepository<TRecord, TMessage> _repository;

		public InMemoryRegistry(IBasicRecordRepository<TRecord, TMessage> repository)
		{
			_repository = repository;
		}

		public TRecord CreateRecord(TMessage message)
		{
			return _repository.Create(message);
		}

		public TRecord Get(Guid id)
		{
			return _repository.Retrieve(id);
		}

		public TRecord MarkAsComplete(Guid id)
		{
			var record = _repository.Retrieve(id);

			record.Completed = true;

			return _repository.Update(record);
		}

		public TRecord MarkAsFailed(Guid id, string message = null, string callStack = null)
		{
			var record = _repository.Retrieve(id);

			record.Completed = true;

			record.Error = true;

			if (!string.IsNullOrEmpty(message))
			{
				record.ErrorMessage = message;
			}

			if (!string.IsNullOrEmpty(callStack))
			{
				record.CallStack = callStack;
			}

			return _repository.Update(record);
		}
	}
}