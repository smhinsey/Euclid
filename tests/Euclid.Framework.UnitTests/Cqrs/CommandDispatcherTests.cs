using System;
using Euclid.Common.Messaging;
using Euclid.Common.Storage;
using Euclid.Framework.Cqrs;
using Euclid.Framework.TestingFakes.Cqrs;
using NUnit.Framework;

namespace Euclid.Framework.UnitTests.Cqrs
{
	[TestFixture]
	public class CommandDispatcherTests
	{
		[Test]
		public void CommandRegistryTests()
		{
			var command = new FakeCommand {Identifier = Guid.NewGuid()};
			var registry = new CommandRegistry(new InMemoryRecordMapper<CommandPublicationRecord>(), new InMemoryBlobStorage(), new JsonMessageSerializer());

			var record = registry.CreateRecord(command);
			Assert.NotNull(record);

			var retrieved = registry.GetMessage(record.MessageLocation, record.MessageType);
			Assert.NotNull(retrieved);
			Assert.AreEqual(command.Identifier, retrieved.Identifier);

			Assert.NotNull(retrieved as FakeCommand);
		}
	}
}