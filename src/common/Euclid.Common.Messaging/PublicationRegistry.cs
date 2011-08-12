﻿using System;
using Euclid.Common.Storage;
using Euclid.Common.Storage.Binary;
using Euclid.Common.Storage.Record;

namespace Euclid.Common.Messaging
{
	public class PublicationRegistry<TRecord> : IPublicationRegistry<TRecord>
		where TRecord : class, IPublicationRecord, new()
	{
		protected readonly IBlobStorage BlobStorage;
		protected readonly IRecordMapper<TRecord> Mapper;
		protected readonly IMessageSerializer Serializer;


		public PublicationRegistry(IRecordMapper<TRecord> mapper, IBlobStorage blobStorage, IMessageSerializer serializer)
		{
			Mapper = mapper;
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

			var record = new TRecord
			             	{
			             		Identifier = Guid.NewGuid(),
			             		Created = DateTime.Now,
			             		MessageLocation = uri,
			             		MessageType = message.GetType()
			             	};

			return Mapper.Create(record);
		}

		public virtual IMessage GetMessage(Uri messageLocation, Type recordType)
		{
			var messageBlob = BlobStorage.Get(messageLocation);

			return Convert.ChangeType(Serializer.Deserialize(messageBlob.Bytes), recordType) as IMessage;
		}

		public virtual TRecord MarkAsComplete(Guid id)
		{
			return updateRecord
				(id, r =>
				     	{
				     		r.Completed = true;
				     		r.Dispatched = true;
				     		r.Error = false;
				     	});
		}

		public virtual TRecord MarkAsFailed(Guid id, string message, string callStack)
		{
			return updateRecord
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
			return updateRecord
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
			return Mapper.Retrieve(identifier);
		}

		private TRecord updateRecord(Guid id, Action<TRecord> actOnRecord)
		{
			var record = GetRecord(id);

			actOnRecord(record);

			return Mapper.Update(record);
		}
	}
}