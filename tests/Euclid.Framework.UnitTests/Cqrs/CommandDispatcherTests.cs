using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Euclid.Common.Messaging;
using Euclid.Common.Storage.Blob;
using Euclid.Common.Storage.Record;
using Euclid.Framework.Cqrs;
using Euclid.Framework.TestingFakes.Cqrs;
using NUnit.Framework;

namespace Euclid.Framework.UnitTests.Cqrs
{
    [TestFixture]
    public class CommandDispatcherTests
    {
        [Test]
        public void CommandPublicationRecordCreates()
        {
            var command = new FakeCommand { Identifier = Guid.NewGuid()};
            var registry = new CommandRegistry(new InMemoryRecordRepository<CommandPublicationRecord>(), new InMemoryBlobStorage(), new JsonMessageSerializer());

            var record = registry.CreateRecord(command);
            Assert.NotNull(record);

            var retrieved = registry.GetMessage(record.MessageLocation, record.MessageType);
            Assert.NotNull(retrieved);
            Assert.AreEqual(command.Identifier, retrieved.Identifier);

            Assert.NotNull(retrieved as FakeCommand);
        }
    }
}
