using System;
using Euclid.Common.Registry;
using Euclid.Common.Storage;
using Euclid.Common.TestingFakes.Registry;
using Euclid.Common.Transport;
using NUnit.Framework;

namespace Euclid.Common.IntegrationTests
{
	[TestFixture]
	public class PublicationTests
	{
		private readonly IRegistry<FakeRecord, FakeMessage> _registry;
		private readonly IMessageTransport _transport;
		private readonly IBasicRecordRepository<FakeRecord, FakeMessage> _repository;

		public PublicationTests()
		{
			_repository = new InMemoryRecordRepository<FakeRecord, FakeMessage>();
			_registry = new FakeRegistry(_repository);
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

			var innerMessage = (receivedMsg as FakeRecord).Message;

			Assert.NotNull(innerMessage);

			Assert.AreEqual(msgId, innerMessage.Identifier);

			Assert.AreEqual(createdById, innerMessage.CreatedBy);

			Assert.AreEqual(created, innerMessage.Created);

			var d = _registry.CreateRecord(receivedMsg as FakeMessage);

			Assert.NotNull(d);

			_transport.Close();
		}
	}
}