using System;
using System.Linq;
using System.Reflection;
using Euclid.Framework.Agent.Extensions;
using Euclid.Framework.Agent.Metadata;
using Euclid.Framework.Cqrs;
using Euclid.Framework.Models;
using Euclid.Sdk.FakeAgent.Commands;
using Euclid.Sdk.FakeAgent.Queries;
using Euclid.Sdk.FakeAgent.ReadModels;
using Euclid.TestingSupport;
using NUnit.Framework;

namespace Euclid.Framework.UnitTests.Metadata
{
	[TestFixture]
	[Category(TestCategories.Unit)]
	public class AgentMetadataTests
	{
		private void testAgentParts(IAgentPartMetadataFormatterCollection partMetadataFormatterCollection, Assembly agent, Type testType)
		{
			Assert.NotNull(partMetadataFormatterCollection);
			Assert.GreaterOrEqual(partMetadataFormatterCollection.Count(), 1);

			Assert.True(!string.IsNullOrEmpty(partMetadataFormatterCollection.Namespace));
			Assert.NotNull(agent.GetTypes().Where(x => x.Namespace == partMetadataFormatterCollection.Namespace).FirstOrDefault());

			var partMetadata = partMetadataFormatterCollection.Where(x => x.Name == testType.Name && x.Namespace == partMetadataFormatterCollection.Namespace).FirstOrDefault();
			Assert.NotNull(partMetadata);

			var typeMetadata = partMetadataFormatterCollection.GetMetadata(testType);
			Assert.NotNull(typeMetadata);
			Assert.AreEqual(typeMetadata.Type, partMetadata.Type);
		}

		[Test]
		public void TestComposition()
		{
			var assembly = typeof (FakeCommand).Assembly;
			var agentMetadata = assembly.GetAgentMetadata();
			Assert.NotNull(agentMetadata);
			Assert.True(agentMetadata.IsValid);

			testAgentParts(agentMetadata.Commands, assembly, typeof (FakeCommand));
			testAgentParts(agentMetadata.Queries, assembly, typeof (FakeQuery));
			testAgentParts(agentMetadata.ReadModels, assembly, typeof (FakeReadModel));
		}

		[Test]
		public void TestPartInheritance()
		{
			Assert.True(typeof (IAgentPart).IsAssignableFrom(typeof (ICommand)));
			Assert.True(typeof (IAgentPart).IsAssignableFrom(typeof (IQuery<IReadModel>)));
			Assert.True(typeof (IAgentPart).IsAssignableFrom(typeof (ICommandProcessor<ICommand>)));
		}
	}
}