using Euclid.Common.Registry;
using Euclid.Common.Serialization;
using Euclid.Common.Storage;

namespace Euclid.Common.TestingFakes.Registry
{
	public class FakeRegistry : DefaultRecordRegistry<FakeRecord>
	{
		public FakeRegistry(IBasicRecordRepository<FakeRecord> repository, IBlobStorage storage, IMessageSerializer serializer) : base(repository, storage, serializer)
		{
		}
	}
}