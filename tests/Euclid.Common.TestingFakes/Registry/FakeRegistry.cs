using Euclid.Common.Registry;

namespace Euclid.Common.TestingFakes.Registry
{
    public class FakeRegistry : InMemoryRegistry<FakeRecord, FakeMessage>
    {
        public FakeRegistry(IBasicRecordRepository<FakeRecord, FakeMessage> repository) : base(repository)
        {
        }
    }
}