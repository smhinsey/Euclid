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
		private readonly IPublicationRegistry<FakePublicationRecord> _publicationRegistry;
		private readonly IMessageChannel _channel;
		private readonly IRecordMapper<FakePublicationRecord> _mapper;
		private readonly IBlobStorage _blobStorage;
		private readonly IMessageSerializer _serializer;

		public PublicationTests()
		{
			_serializer = new JsonMessageSerializer();
			_blobStorage = new InMemoryBlobStorage();
			_mapper = new InMemoryRecordMapper<FakePublicationRecord>();
			_publicationRegistry = new FakeRegistry(_mapper, _blobStorage, _serializer);
			_channel = new InMemoryMessageChannel();
		}

		[Test]
		public void TestSendMessageOverTransport()
		{
			_channel.Open();

			var msgId = Guid.NewGuid();

			var createdById = Guid.NewGuid();

			var created = DateTime.Now;

			var msg = new FakeMessage
			          	{
			          		Created = created,
			          		CreatedBy = createdById,
			          		Identifier = msgId
			          	};

			var record = _publicationRegistry.CreateRecord(msg);

			_channel.Send(record);

			var receivedMsg = _channel.ReceiveSingle(TimeSpan.MaxValue);

			Assert.NotNull(receivedMsg);

			Assert.NotNull(receivedMsg as FakePublicationRecord);

			var receivedRecord = receivedMsg as FakePublicationRecord;

			Assert.AreEqual(typeof (FakeMessage), receivedRecord.MessageType);

			var blob = _blobStorage.Get(receivedRecord.MessageLocation);

			Assert.NotNull(blob);

			var storedMessage = Convert.ChangeType(_serializer.Deserialize(blob.Bytes), receivedRecord.MessageType);

			Assert.NotNull(storedMessage);

			Assert.AreEqual(typeof (FakeMessage), storedMessage.GetType());

			_channel.Close();
		}
	}
}