using System;
using System.Collections.Generic;
using Euclid.Common.Messaging;
using Euclid.Sdk.FakeAgent.Commands;
using Euclid.Sdk.FakeAgent.Queries;
using Euclid.TestingSupport;
using NUnit.Framework;

namespace Euclid.Sdk.IntegrationTests
{
	[Category(TestCategories.Integration)]
	public class HostingFabricTests : HostingFabricFixture
	{
		public HostingFabricTests()
			: base(typeof (FakeCommand).Assembly)
		{
		}

		[Test]
		public void PublishProcessAndCompleteManyCommands()
		{
			var publicationIds = new List<Guid>();
			const int numberOfCommands = 100;

			var publisher = Container.Resolve<IPublisher>();

			for (var i = 0; i < numberOfCommands; i++)
			{
				var publicationId = publisher.PublishMessage(new FakeCommand {Number = i});

				publicationIds.Add(publicationId);
			}

			foreach (var publicationId in publicationIds)
			{
				WaitUntilComplete(publicationId);
			}
		}

		[Test]
		public void PublishProcessAndVerifyCommandByQuery()
		{
			const int messageNumber = 134;

			var publisher = Container.Resolve<IPublisher>();

			WaitUntilComplete(
			                  publisher.PublishMessage(new FakeCommand {Number = messageNumber}));

			var query = Container.Resolve<FakeQuery>();

			var models = query.FindByNumber(messageNumber);

			Assert.AreEqual(1, models.Count);
			Assert.AreEqual(messageNumber, models[0].Number);
		}
	}
}