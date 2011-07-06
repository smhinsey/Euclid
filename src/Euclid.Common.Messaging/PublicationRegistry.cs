﻿using System;
using Euclid.Common.Storage;
using Euclid.Common.Storage.Blob;

namespace Euclid.Common.Messaging
{
	public class PublicationRegistry<TRecord> : IPublicationRegistry<TRecord>
		where TRecord : class, IPublicationRecord, new()
	{
		protected readonly IBlobStorage BlobStorage;
		protected readonly IBasicRecordRepository<TRecord> Repository;
		protected readonly IMessageSerializer Serializer;


		public PublicationRegistry(IBasicRecordRepository<TRecord> repository, IBlobStorage blobStorage, IMessageSerializer serializer)
		{
			Repository = repository;
			BlobStorage = blobStorage;
			Serializer = serializer;
		}

		public virtual TRecord CreateRecord(IMessage message)
		{
			var msgBlob = new Blob
			              	{
			              		Bytes = Serializer.Serialize(message),
			              		ContentType = "application/octet-stream"
			              	};

			var uri = BlobStorage.Put(msgBlob, message.GetType().FullName);

			return Repository.Create(uri, message.GetType());
		}

		public virtual IMessage GetMessage(Uri messageLocation, Type recordType)
		{
			var messageBlob = BlobStorage.Get(messageLocation);

			return Convert.ChangeType(Serializer.Deserialize(messageBlob.Bytes), recordType) as IMessage;
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
			return Repository.Retrieve(identifier);
		}

		private TRecord UpdateRecord(Guid id, Action<TRecord> actOnRecord)
		{
			var record = GetRecord(id);

			actOnRecord(record);

			return Repository.Update(record);
		}
	}
}