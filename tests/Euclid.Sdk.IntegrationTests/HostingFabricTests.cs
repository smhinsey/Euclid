using System;
using System.Collections.Generic;
using System.Threading;
using Euclid.Common.Messaging;
using Euclid.Framework.Cqrs;
using Euclid.Sdk.FakeAgent.Commands;
using Euclid.Sdk.FakeAgent.Queries;
using NUnit.Framework;

namespace Euclid.Sdk.IntegrationTests
{
	public class HostingFabricTests : HostingFabricFixture
	{
		[Test]
		public void PublishProcessAndCompleteManyCommands()
		{
			var publisher = Container.Resolve<IPublisher>();

			var publicationIds = new List<Guid>();

			for (var i = 0; i < 10; i++)
			{
				var publicationId = publisher.PublishMessage(new FakeCommand {Number = i});

				publicationIds.Add(publicationId);
			}

			Thread.Sleep(15000);

			var registry = Container.Resolve<ICommandRegistry>();

			foreach (var publicationId in publicationIds)
			{
				var record = registry.GetRecord(publicationId);

				Assert.IsTrue(record.Completed, "Publication record was marked complete.");
			}
		}

		[Test]
		public void PublishProcessAndVerifyCommandByQuery()
		{
			const int messageNumber = 134;

			var publisher = Container.Resolve<IPublisher>();

			publisher.PublishMessage(new FakeCommand {Number = messageNumber});

			Thread.Sleep(15000);

			var query = Container.Resolve<FakeQuery>();

			var models = query.FindByNumber(messageNumber);

			Assert.AreEqual(1, models.Count);
			Assert.AreEqual(messageNumber, models[0].Number);
		}
	}
}