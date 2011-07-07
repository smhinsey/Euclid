using Euclid.Common.Messaging;
using Euclid.Common.Storage;

namespace Euclid.Common.TestingFakes.Registry
{
	public class FakeRegistry : PublicationRegistry<FakePublicationRecord>
	{
		public FakeRegistry(IBasicRecordMapper<FakePublicationRecord> mapper, IBlobStorage storage, IMessageSerializer serializer) : base(mapper, storage, serializer)
		{
		}
	}
}