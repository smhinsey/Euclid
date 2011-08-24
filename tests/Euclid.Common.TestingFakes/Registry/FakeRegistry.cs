using Euclid.Common.Messaging;
using Euclid.Common.Storage.Binary;
using Euclid.Common.Storage.Record;

namespace Euclid.Common.TestingFakes.Registry
{
	public class FakeRegistry : PublicationRegistry<FakePublicationRecord, FakePublicationRecord>
	{
		public FakeRegistry(IRecordMapper<FakePublicationRecord> mapper, IBlobStorage storage, IMessageSerializer serializer)
			: base(mapper, storage, serializer)
		{
		}
	}
}
