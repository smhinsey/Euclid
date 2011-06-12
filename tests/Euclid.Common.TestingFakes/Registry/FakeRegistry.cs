using Euclid.Common.Registry;
using Euclid.Common.Storage;

namespace Euclid.Common.TestingFakes.Registry
{
	public class FakeRegistry : InMemoryRegistry<FakeRecord, FakeMessage>
	{
		public FakeRegistry(IBasicRecordRepository<FakeRecord, FakeMessage> repository) : base(repository)
		{
		}
	}
}