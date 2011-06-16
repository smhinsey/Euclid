using System;
using System.IO;
using Euclid.Common.Registry;
using Euclid.Common.Serialization;
using Euclid.Common.Storage;
using Euclid.Common.Storage.Blob;
using Euclid.Common.Storage.Record;
using Euclid.Common.TestingFakes.Registry;
using Euclid.Common.Transport;
using NUnit.Framework;

namespace Euclid.Common.IntegrationTests
{
	[TestFixture]
	public class PublicationTests
	{
		private readonly IRegistry<FakeRecord> _registry;
		private readonly IMessageTransport _transport;
		private readonly IBasicRecordRepository<FakeRecord> _repository;
		private readonly IBlobStorage _blobStorage;
		private readonly IMessageSerializer _serializer;

		public PublicationTests()
		{
			_serializer = new JsonMessageSerializer();
			_blobStorage = new InMemoryBlobStorage();
			_repository = new InMemoryRecordRepository<FakeRecord>();
			_registry = new FakeRegistry(_repository, _blobStorage, _serializer);
			_transport = new InMemoryMessageTransport();
		}

		[Test]
		public void TestSendMessageOverTransport()
		{
			_transport.Open();

			var msgId = Guid.NewGuid();

			var createdById = Guid.NewGuid();

			var created = DateTime.Now;

			var msg = new FakeMessage
			          	{
			          		Created = created,
			          		CreatedBy = createdById,
			          		Identifier = msgId
			          	};

			var record = _registry.CreateRecord(msg);

			_transport.Send(record);

			var receivedMsg = _transport.ReceiveSingle(TimeSpan.MaxValue);

			Assert.NotNull(receivedMsg);

			Assert.NotNull(receivedMsg as FakeRecord);

			var receivedRecord = receivedMsg as FakeRecord;

			Assert.AreEqual(typeof (FakeMessage), receivedRecord.MessageType);

			var blobBytes = _blobStorage.Get(receivedRecord.MessageLocation);

			Assert.NotNull(blobBytes);

			var blobStream = new MemoryStream(blobBytes);

			var storedMessage = Convert.ChangeType(_serializer.Deserialize(blobStream), receivedRecord.MessageType);

			Assert.NotNull(storedMessage);

			Assert.AreEqual(typeof (FakeMessage), storedMessage.GetType());

			_transport.Close();
		}
	}
}