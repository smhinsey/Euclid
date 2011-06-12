using Euclid.Common.Registry;
using Euclid.Common.TestingFakes.Registry;

namespace Euclid.Common.UnitTests.Registry
{
    public interface ITestRegistry
    {
        IRegistry<FakeRecord, FakeMessage> Registry { get; }

    }
}