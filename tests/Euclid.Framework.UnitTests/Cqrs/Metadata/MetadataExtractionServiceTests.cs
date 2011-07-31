using NUnit.Framework;

namespace Euclid.Framework.UnitTests.Cqrs.Metadata
{
    [TestFixture]
    public class MetadataExtractionServiceTests
    {
        //[Test]
        //public void ExtractsCommandData()
        //{
        //    var settings = new MetadataServiceSettings(new List<Assembly> {typeof (FakeCommand).Assembly});
        //    var svc = new CommandMetadataService(settings);
        //    var commands = svc.GetVisibleCommandTypes();

        //    Assert.AreEqual(3, commands.Count());
        //    Assert.NotNull(commands.Where(t => t == typeof (FakeCommand)).First());
        //    Assert.NotNull(commands.Where(t => t == typeof (FakeCommand2)).First());
        //}

        //[Test]
        //public void GetCommand()
        //{
        //    var settings = new MetadataServiceSettings(new List<Assembly> {typeof (FakeCommand).Assembly});
        //    var svc = new CommandMetadataService(settings);
        //    var commands = svc.GetVisibleCommandTypes();

        //    Assert.AreEqual(3, commands.Count());
        //    var fakeCommandType = commands.Where(t => t == typeof (FakeCommand)).First();
        //    Assert.NotNull(fakeCommandType);

        //    var fakeCommand = svc.CreateCommand(fakeCommandType);
        //    Assert.NotNull(fakeCommand);

        //    fakeCommandType = commands.Where(t => t == typeof (FakeCommand2)).First();
        //    Assert.NotNull(fakeCommandType);

        //    fakeCommand = svc.CreateCommand(fakeCommandType);
        //    Assert.NotNull(fakeCommand);
        //}
    }
}