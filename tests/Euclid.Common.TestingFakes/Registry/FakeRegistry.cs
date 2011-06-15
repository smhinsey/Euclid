using Euclid.Common.Registry;
using Euclid.Common.Storage;

namespace Euclid.Common.TestingFakes.Registry
{
	public class FakeRegistry : InMemoryRegistry<FakeRecord>
	{
		public FakeRegistry(IBasicRecordRepository<FakeRecord> repository) : base(repository)
		{
		}
	}
}