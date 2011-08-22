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
			: base(typeof(FakeCommand).Assembly)
		{
		}

		[Test]
		public void PublishProcessAndCompleteManyCommands()
		{
			var publicationIds = new List<Guid>();
			const int NumberOfCommands = 100;

			var publisher = this.Container.Resolve<IPublisher>();

			for (var i = 0; i < NumberOfCommands; i++)
			{
				var publicationId = publisher.PublishMessage(new FakeCommand { Number = i });

				publicationIds.Add(publicationId);
			}

			foreach (var publicationId in publicationIds)
			{
				this.WaitUntilComplete(publicationId);
			}
		}

		[Test]
		public void PublishProcessAndVerifyCommandByQuery()
		{
			const int MessageNumber = 134;

			var publisher = this.Container.Resolve<IPublisher>();

			this.WaitUntilComplete(publisher.PublishMessage(new FakeCommand { Number = MessageNumber }));

			var query = this.Container.Resolve<FakeQuery>();

			var models = query.FindByNumber(MessageNumber);

			Assert.AreEqual(1, models.Count);
			Assert.AreEqual(MessageNumber, models[0].Number);
		}
	}
}