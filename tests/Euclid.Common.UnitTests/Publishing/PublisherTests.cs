﻿using System;
using Euclid.Common.Messaging;
using Euclid.Common.Storage.Blob;
using Euclid.Common.Storage.Record;
using Euclid.Common.TestingFakes.Storage;
using NUnit.Framework;

namespace Euclid.Common.UnitTests.Publishing
{
	[TestFixture]
	internal class PublisherTests
	{
		[Test]
		public void Publishes()
		{
			var repository = new InMemoryRecordRepository<FakePublicationRecord>();
			var blobStorage = new InMemoryBlobStorage();
			var serializer = new JsonMessageSerializer();
			var transport = new InMemoryMessageChannel();
			var registry = new PublicationRegistry<FakePublicationRecord>(repository, blobStorage, serializer);

			var publisher = new EuclidPublisher(registry, transport);

			var start = DateTime.Now;
			var createdBy = new Guid("C60696AF-F2F8-44EB-B9A1-7967693AC466");
			var identifier = new Guid("EF994892-F442-4681-AD1C-217BB11A6D38");
			var message = new FakeMessage
			              	{
			              		Created = start,
			              		CreatedBy = createdBy,
			              		Identifier = identifier
			              	};

			var recordId = publisher.PublishMessage(message);
			Assert.NotNull(recordId);
			Assert.AreNotEqual(Guid.Empty, recordId);

			var record = registry.GetRecord(recordId);
			Assert.NotNull(record);
			Assert.AreEqual(typeof (FakeMessage), record.MessageType);

			var retrieved = registry.GetMessage(record.MessageLocation, record.MessageType);
			Assert.NotNull(retrieved);
			Assert.AreEqual(message.Identifier, retrieved.Identifier);
			Assert.AreEqual(message.Created.ToString(), retrieved.Created.ToString());
			Assert.AreEqual(message.Identifier, retrieved.Identifier);
		}
	}
}