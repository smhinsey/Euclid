using System.Linq;
using Euclid.Composites.Agent;
using Euclid.Composites.Extensions;
using Euclid.Composites.Metadata;
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

            var command = agentInfo.GetCommand<FakeCommand>();

            Assert.NotNull(command);

            Assert.AreEqual(typeof(FakeCommand), command.GetType());

            command = agentInfo.GetCommand("FakeCommand");

            Assert.NotNull(command);

            Assert.AreEqual(typeof(FakeCommand), command.GetType());
        }

        [Test]
        public void TestGetCommands()
        {
            var agentInfo = typeof(FakeCommand).Assembly.GetAgentInfo();

            var commands = agentInfo.GetCommands();

            Assert.NotNull(commands);

            Assert.GreaterOrEqual(commands.Count(), 1);
        }
         
    }
}