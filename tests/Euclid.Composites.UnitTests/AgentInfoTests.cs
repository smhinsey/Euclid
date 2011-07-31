using System.Linq;
using Euclid.Composites.Agent;
using Euclid.Composites.Extensions;
using Euclid.Framework.Metadata.Extensions;
using Euclid.SDK.TestingFakes.Composites;
using NUnit.Framework;

namespace Euclid.Composites.UnitTests
{
    [TestFixture]
    public class AgentInfoTests
    {
        [Test]
        public void TestAgentInfo()
        {
            var agentInfo = typeof(FakeCommand).Assembly.GetAgentInfo();

            Assert.NotNull(agentInfo);
        }

        [Test]
        public void TestGetCommand()
        {
            var agentInfo = typeof(FakeCommand).Assembly.GetAgentInfo();

            Assert.True(agentInfo.SupportsCommand<FakeCommand>());

            var commandMetadata = agentInfo.GetCommandMetadata<FakeCommand>();

            Assert.NotNull(commandMetadata);

            Assert.AreEqual(typeof(FakeCommand), commandMetadata.Type);

            var command = agentInfo.GetCommand(commandMetadata.Type);

            Assert.NotNull(command);

            Assert.AreEqual(typeof(FakeCommand), command.GetType());

            commandMetadata = agentInfo.GetCommandMetadata("FakeCommand");

            Assert.NotNull(commandMetadata);

            Assert.AreEqual(typeof(FakeCommand), commandMetadata.Type);
        }

        [Test]
        public void TestGetCommands()
        {
            var agentInfo = typeof(FakeCommand).Assembly.GetAgentInfo();

            Assert.NotNull(agentInfo.Commands);

            Assert.GreaterOrEqual(agentInfo.Commands.Count(), 1);
        }
         
    }
}