using System;
using System.Reflection;
using Euclid.Composites.Agent;
using Euclid.Framework.Cqrs;
﻿using Euclid.Framework.Metadata;
﻿using Euclid.Framework.Metadata.Extensions;
﻿using Euclid.SDK.TestingFakes.Composites;
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
            var metadata = new EuclidMetadata(typeof (FakeCommand));

            Assert.AreEqual(3, metadata.Interfaces.Count);
            Assert.True(metadata.Interfaces.Any(x => x.InterfaceName == "ICommand"));
            Assert.True(metadata.Interfaces.Any(x => x.InterfaceName == "IMessage"));
            Assert.True(metadata.Interfaces.Any(x => x.InterfaceName == "IFakeMarker"));

            Assert.AreEqual(5, metadata.Properties.Count);
            var p = metadata.Properties.Where(x => x.Name == "Created").FirstOrDefault();
            Assert.NotNull(p);
            Assert.AreEqual(p.Name, "Created");
            Assert.AreEqual(p.PropertyType, typeof (DateTime));
            //Assert.AreEqual(2, p.CustomAttributes.Count);
            //Assert.True(p.CustomAttributes.Any(x => x.Name == "FakeAttribute"));
            //Assert.True(p.CustomAttributes.Any(x => x.Name == "DescriptionAttribute"));

            Assert.True(metadata.Properties.Any(prop => prop.Name == "CreatedBy" && prop.PropertyType == typeof (Guid)));
            Assert.True(metadata.Properties.Any(prop => prop.Name == "Identifier" && prop.PropertyType == typeof(Guid)));

        }

        [Test]
        public void TestAssemblyMetadata()
        {
            var assembly = typeof (FakeCommand).Assembly;
            var agentInfo = assembly.GetAgentInfo();

            Assert.True(assembly.ContainsAgent());

            TestFakeAgent(assembly);
        }

        [Test]
        public void TestResolveAgentFromFileSystem()
        {
            var resolver = new FileSystemAgentResolver();

            var agent = resolver.GetAgent("Fake");

            Assert.NotNull(agent);

            TestFakeAgent(agent);
        }

        [Test]
        public void TestResolveAgentFromAppDomain()
        {
            var resolver = new AppDomainAgentResolver();

            var assembly = typeof (FakeCommand).Assembly;

            var agent = resolver.GetAgent("Fake");

            Assert.NotNull(agent);

            TestFakeAgent(agent);
        }

        [Test]
        public void GetListOfCommandsFromAgent()
        {
            var resolver = new FileSystemAgentResolver();

            var agent = resolver.GetAgent("Fake");

            var agentMetadata = agent.GetAgentInfo();

            var commandTypes =
                agent.GetTypes().Where(
                    x => x.Namespace == agentMetadata.CommandNamespace && typeof (ICommand).IsAssignableFrom(x)).ToList();

            Assert.Contains(typeof (FakeCommand), commandTypes);
        }

        private static void TestFakeAgent(Assembly agent)
        {
            var metadata = agent.GetAgentInfo();
            Assert.AreEqual("Fake Agent", metadata.FriendlyName);
            Assert.AreEqual("Fake", metadata.SystemName);
            Assert.AreEqual("Euclid.SDK.TestingFakes.Composites", metadata.CommandNamespace);
            Assert.AreEqual("FakeAgent.Queries", metadata.QueryNamespace);
            Assert.AreEqual("FakeAgent.Processors", metadata.CommandProcessorNamespace);
        }
    }
}