using Euclid.Common.Messaging;
using Euclid.Common.Storage;

namespace Euclid.Common.TestingFakes.Registry
{
	public class FakeRegistry : PublicationRegistry<FakePublicationRecord>
	{
		public FakeRegistry(IBasicRecordRepository<FakePublicationRecord> repository, IBlobStorage storage, IMessageSerializer serializer) : base(repository, storage, serializer)
		{
		}
	}
}