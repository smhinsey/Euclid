using System;
using Euclid.Common.Storage;
using Euclid.Common.Storage.Binary;
using Euclid.Common.Storage.Record;

namespace Euclid.Common.Messaging
{
	public class PublicationRegistry<TRecord, TRecordContract> : IPublicationRegistry<TRecord, TRecordContract>
		where TRecord : class, TRecordContract, IPublicationRecord, new() where TRecordContract : IPublicationRecord
	{
		protected readonly IBlobStorage BlobStorage;

		protected readonly IRecordMapper<TRecord> Mapper;

		protected readonly IMessageSerializer Serializer;

		public PublicationRegistry(IRecordMapper<TRecord> mapper, IBlobStorage blobStorage, IMessageSerializer serializer)
		{
			this.Mapper = mapper;
			this.BlobStorage = blobStorage;
			this.Serializer = serializer;
		}

		public virtual IMessage GetMessage(Uri messageLocation, Type recordType)
		{
			var messageBlob = this.BlobStorage.Get(messageLocation);

			return Convert.ChangeType(this.Serializer.Deserialize(messageBlob.Bytes), recordType) as IMessage;
		}

		public TRecordContract GetPublicationRecord(Guid identifier)
		{
			return this.Mapper.Retrieve(identifier);
		}

		public virtual TRecordContract MarkAsComplete(Guid identifier)
		{
			return this.updateRecord(
				identifier, 
				r =>
					{
						r.Completed = true;
						r.Dispatched = true;
						r.Error = false;
					});
		}

		public virtual TRecordContract MarkAsFailed(Guid identifier, string message, string callStack)
		{
			return this.updateRecord(
				identifier, 
				r =>
					{
						r.Dispatched = true;
						r.Completed = true;
						r.Error = true;
						r.ErrorMessage = message;
						r.CallStack = callStack;
					});
		}

		public virtual TRecordContract MarkAsUnableToDispatch(Guid identifier, bool isError = false, string message = null)
		{
			return this.updateRecord(
				identifier, 
				r =>
					{
						r.Dispatched = false;
						r.Completed = true;
						r.Error = isError;
						r.ErrorMessage = string.IsNullOrEmpty(message) ? string.Empty : message;
					});
		}

		public virtual TRecordContract PublishMessage(IMessage message)
		{
			var msgBlob = new Blob { Bytes = this.Serializer.Serialize(message), ContentType = "application/octet-stream" };

			var uri = this.BlobStorage.Put(msgBlob, message.GetType().FullName);

			var record = new TRecord
				{
       Identifier = Guid.NewGuid(), Created = DateTime.Now, MessageLocation = uri, MessageType = message.GetType() 
    };

			return this.Mapper.Create(record);
		}

		private TRecordContract updateRecord(Guid id, Action<TRecordContract> actOnRecord)
		{
			var record = this.GetPublicationRecord(id);

			actOnRecord(record);

			return this.Mapper.Update((TRecord)record);
		}
	}
}