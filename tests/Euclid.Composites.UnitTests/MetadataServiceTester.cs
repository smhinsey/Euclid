using System;
using System.Reflection;
using Euclid.Composites.Agent;
using Euclid.Composites.Extensions;
using Euclid.Composites.Metadata;
using Euclid.Framework.Cqrs;
using Euclid.SDK.TestingFakes.Composites;
using NUnit.Framework;
using System.Linq;

namespace Euclid.Composites.UnitTests
{
    [TestFixture]
    public class MetadataServiceTester
    {
        [Test]
        public void TestCommandMetadata()
        {
            var metadata = new CommandMetadata(typeof(FakeCommand));
            
            Assert.AreEqual(3, metadata.Interfaces.Count);
            Assert.True(metadata.Interfaces.Any(x => x.Name == "ICommand"));
            Assert.True(metadata.Interfaces.Any(x => x.Name == "IMessage"));
            Assert.True(metadata.Interfaces.Any(x => x.Name == "IFakeMarker"));

            Assert.AreEqual(5, metadata.Properties.Count);
            var p = metadata.Properties.Where(x => x.Name == "Created").FirstOrDefault();
            Assert.NotNull(p);
            Assert.AreEqual(p.Name, "Created");
            Assert.AreEqual(p.Type, typeof(DateTime));
            Assert.AreEqual(2, p.CustomAttributes.Count);
            Assert.True(p.CustomAttributes.Any(x=>x.Name =="FakeAttribute"));
            Assert.True(p.CustomAttributes.Any(x=>x.Name == "DescriptionAttribute"));

            Assert.True(metadata.Properties.Any(prop => prop.Name == "CreatedBy" && prop.Type == typeof(Guid)));
            Assert.True(metadata.Properties.Any(prop => prop.Name == "Identifier" && prop.Type == typeof(Guid)));

        }

        [Test]
        public void TestAssemblyMetadata()
        {
            var assembly = typeof (FakeCommand).Assembly; 
            Assert.True(assembly.ContainsAgent());

            TestFakeAgent(assembly);
        }

        [Test]
        public void TestResolveAgentFromFileSystem()
        {
            var resolver = new FileSystemAgentResolver();

            var agent = resolver.GetAgent("Euclid.SDK.TestingFakes.Agent", "Fake");

            Assert.NotNull(agent);

            TestFakeAgent(agent);
        }

        [Test]
        public void TestResolveAgentFromAppDomain()
        {
            var resolver = new AppDomainAgentResolver();

            var assembly = typeof (FakeCommand).Assembly;

            var agent = resolver.GetAgent("Euclid.SDK.TestingFakes.Agent", "Fake");

            Assert.NotNull(agent);

            TestFakeAgent(agent);
        }

        [Test]
        public void GetListOfCommandsFromAgent()
        {
					var resolver = new FileSystemAgentResolver();

            var agent = resolver.GetAgent("Euclid.SDK.TestingFakes.Agent", "Fake");

            var agentMetadata = agent.GetAgentMetadata();

            var commandTypes = agent.GetTypes().Where(x => x.Namespace == agentMetadata.CommandNamespace).ToList();

            Assert.Contains(typeof(FakeCommand), commandTypes);
            //Assert.True(commandTypes.Any(x => x == typeof (FakeCommand)));
        }

        private static void TestFakeAgent(Assembly agent)
        {
            var metadata = agent.GetAgentMetadata();
            Assert.AreEqual("Fake Agent", metadata.FriendlyName);
            Assert.AreEqual("Fake", metadata.SystemName);
            Assert.AreEqual("FakeAgent.Commands", metadata.CommandNamespace);
            Assert.AreEqual("FakeAgent.Queries", metadata.QueryNamespace);
            Assert.AreEqual("FakeAgent.Processors", metadata.CommandProcessorNamespace);
            Assert.AreEqual("Euclid.SDK.TestingFakes.Agent", metadata.Scheme);
        }
    }
}
