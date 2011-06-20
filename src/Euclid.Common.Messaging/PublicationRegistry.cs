using System;
using Euclid.Common.Storage;
using Euclid.Common.Storage.Blob;

namespace Euclid.Common.Messaging
{
	public class PublicationRegistry<TRecord> : IPublicationRegistry<TRecord>
		where TRecord : class, IPublicationRecord, new()
	{
		private readonly IBlobStorage _blobStorage;
		private readonly IBasicRecordRepository<TRecord> _repository;

		private readonly IMessageSerializer _serializer;


		public PublicationRegistry(IBasicRecordRepository<TRecord> repository, IBlobStorage blobStorage, IMessageSerializer serializer)
		{
			_repository = repository;
			_blobStorage = blobStorage;
			_serializer = serializer;
		}

		public virtual TRecord CreateRecord(IMessage message)
		{
			var msgBlob = new Blob
			              	{
			              		Bytes = _serializer.Serialize(message),
			              		ContentType = "application/octet-stream"
			              	};

			var uri = _blobStorage.Put(msgBlob, message.GetType().FullName);

			return _repository.Create(uri, message.GetType());
		}

		public virtual IMessage GetMessage(Uri messageLocation, Type recordType)
		{
			var messageBlob = _blobStorage.Get(messageLocation);

			return Convert.ChangeType(_serializer.Deserialize(messageBlob.Bytes), recordType) as IMessage;
		}

		public virtual TRecord MarkAsComplete(Guid id)
		{
			return UpdateRecord
				(id, r =>
				     	{
				     		r.Completed = true;
				     		r.Dispatched = true;
				     		r.Error = false;
				     	});
		}

		public virtual TRecord MarkAsFailed(Guid id, string message, string callStack)
		{
			return UpdateRecord
				(id, r =>
				     	{
				     		r.Dispatched = true;
				     		r.Completed = true;
				     		r.Error = true;
				     		r.ErrorMessage = message;
				     		r.CallStack = callStack;
				     	});
		}

		public virtual TRecord MarkAsUnableToDispatch(Guid recordId, bool isError = false, string message = null)
		{
			return UpdateRecord
				(recordId, r =>
				           	{
				           		r.Dispatched = false;
				           		r.Completed = true;
				           		r.Error = isError;
				           		r.ErrorMessage = string.IsNullOrEmpty(message) ? string.Empty : message;
				           	});
		}

		public TRecord GetRecord(Guid identifier)
		{
			return _repository.Retrieve(identifier);
		}

		private TRecord UpdateRecord(Guid id, Action<TRecord> actOnRecord)
		{
			var record = GetRecord(id);

			actOnRecord(record);

			return _repository.Update(record);
		}
	}
}