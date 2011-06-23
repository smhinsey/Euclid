using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Euclid.Framework.Cqrs.Metadata;
using Euclid.Framework.TestingFakes.Cqrs;
using NUnit.Framework;

namespace Euclid.Framework.UnitTests.Cqrs.Metadata
{
    [TestFixture]
    public class MetadataExtractionServiceTests
    {
        [Test]
        public void ExtractsCommandData()
        {
            var settings = new MetadataServiceSettings(new List<Assembly> {typeof (FakeCommand).Assembly});
            var svc = new MetadataService(settings);
            var commands = svc.GetVisibleCommandTypes();

            Assert.AreEqual(2, commands.Count());
            Assert.NotNull(commands.Where(t=>t == typeof(FakeCommand)).First());
            Assert.NotNull(commands.Where(t => t == typeof(FakeCommand2)).First());
        }

        [Test]
        public void GetCommand()
        {
            var settings = new MetadataServiceSettings(new List<Assembly> { typeof(FakeCommand).Assembly });
            var svc = new MetadataService(settings);
            var commands = svc.GetVisibleCommandTypes();

            Assert.AreEqual(2, commands.Count());
            var fakeCommandType = commands.Where(t => t == typeof(FakeCommand)).First();
            Assert.NotNull(fakeCommandType);

            var fakeCommand = svc.CreateCommand(fakeCommandType);
            Assert.NotNull(fakeCommand);

            fakeCommandType = commands.Where(t => t == typeof (FakeCommand2)).First();
            Assert.NotNull(fakeCommandType);

            fakeCommand = svc.CreateCommand(fakeCommandType);
            Assert.NotNull(fakeCommand);
        }
    }
}
