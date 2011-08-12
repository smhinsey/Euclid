using System;
using System.Linq;
using System.Reflection;
using Euclid.Agent.Extensions;
using Euclid.Framework.Agent.Metadata;
using Euclid.Framework.Cqrs;
using Euclid.Framework.Models;
using Euclid.Framework.TestingFakes.Cqrs;
using NUnit.Framework;

namespace Euclid.Framework.UnitTests.Metadata
{
	[TestFixture]
	public class AgentMetadataTests
	{
		private void TestAgentParts(IAgentPartMetadataCollection partMetadataCollection, Assembly agent, IAgentPart testWith = null)
		{
			if (testWith == null)
			{
				Assert.Fail("testwith is null");
			}

			var testType = testWith.GetType();

			Assert.NotNull(partMetadataCollection);
			Assert.GreaterOrEqual(partMetadataCollection.Count(), 1);

			Assert.True(!string.IsNullOrEmpty(partMetadataCollection.Namespace));
			Assert.NotNull(agent.GetTypes().Where(x => x.Namespace == partMetadataCollection.Namespace).FirstOrDefault());

			var partMetadata = partMetadataCollection.Where(x => x.Name == testType.Name && x.Namespace == partMetadataCollection.Namespace).FirstOrDefault();
			Assert.NotNull(partMetadata);

			var typeMetadata = partMetadataCollection.GetMetadata(testType);
			Assert.NotNull(typeMetadata);
			Assert.AreEqual(typeMetadata.Type, partMetadata.Type);
		}

		private void TestCommandMetadataCollection(ICommandMetadataCollection commands)
		{
			//Assert.NotNull(commands);

			//var i = commands.GetInputModel(typeof (FakeCommand4));
			//Assert.NotNull(i);

			//var pwd = i.Properties.Where(p => p.Name == "Password").FirstOrDefault();
			//Assert.NotNull(pwd);
			//Assert.AreEqual(typeof(string), pwd.PropertyType);

			//var c = i.Properties.Where(p => p.Name == "Confirm Password").FirstOrDefault();
			//Assert.NotNull(c);
			//Assert.AreEqual(typeof(string), c.PropertyType);

			//var b = i.Properties.Where(p => p.Name == "Your Birthday").FirstOrDefault();
			//Assert.NotNull(b);
			//Assert.AreEqual(typeof(DateTime), b.PropertyType);
		}

		[Test]
		public void TestComposition()
		{
			var assembly = typeof (FakeCommand).Assembly;
			var agentMetadata = assembly.GetAgentMetadata();
			Assert.NotNull(agentMetadata);
			Assert.True(agentMetadata.IsValid);

			TestAgentParts(agentMetadata.Commands, assembly, new FakeCommand());
			TestCommandMetadataCollection(agentMetadata.Commands);

			Assert.Throws<NotImplementedException>(() => TestAgentParts(agentMetadata.Queries, assembly));
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