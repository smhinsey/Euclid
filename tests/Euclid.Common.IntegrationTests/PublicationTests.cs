using System;
using Euclid.Common.Messaging;
using Euclid.Common.Storage;
using Euclid.Common.Storage.Binary;
using Euclid.Common.Storage.Record;
using Euclid.Common.TestingFakes.Registry;
using Euclid.TestingSupport;
using NUnit.Framework;

namespace Euclid.Common.IntegrationTests
{
	[TestFixture]
	[Category(TestCategories.Integration)]
	public class PublicationTests
	{
		private readonly IBlobStorage _blobStorage;

		private readonly IMessageChannel _channel;

		private readonly IRecordMapper<FakePublicationRecord> _mapper;

		private readonly IPublicationRegistry<FakePublicationRecord, FakePublicationRecord> _publicationRegistry;

		private readonly IMessageSerializer _serializer;

		public PublicationTests()
		{
			this._serializer = new JsonMessageSerializer();
			this._blobStorage = new InMemoryBlobStorage();
			this._mapper = new InMemoryRecordMapper<FakePublicationRecord>();
			this._publicationRegistry = new FakeRegistry(this._mapper, this._blobStorage, this._serializer);
			this._channel = new InMemoryMessageChannel();
		}

		[Test]
		public void TestSendMessageOverTransport()
		{
			this._channel.Open();

			var msgId = Guid.NewGuid();

			var createdById = Guid.NewGuid();

			var created = DateTime.Now;

			var msg = new FakeMessage { Created = created, CreatedBy = createdById, Identifier = msgId };

			var record = this._publicationRegistry.PublishMessage(msg);

			this._channel.Send(record);

			var receivedMsg = this._channel.ReceiveSingle(TimeSpan.MaxValue);

			Assert.NotNull(receivedMsg);

			Assert.NotNull(receivedMsg as FakePublicationRecord);

			var receivedRecord = receivedMsg as FakePublicationRecord;

			Assert.AreEqual(typeof(FakeMessage), receivedRecord.MessageType);

			var blob = this._blobStorage.Get(receivedRecord.MessageLocation);

			Assert.NotNull(blob);

			var storedMessage = Convert.ChangeType(this._serializer.Deserialize(blob.Bytes), receivedRecord.MessageType);

			Assert.NotNull(storedMessage);

			Assert.AreEqual(typeof(FakeMessage), storedMessage.GetType());

			this._channel.Close();
		}
	}
}