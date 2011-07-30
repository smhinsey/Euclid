using Euclid.Common.Storage;
using Euclid.Framework.Cqrs;

namespace Euclid.Composites.MetadataHost
{
    // JT: this is an example of the composite app configuring it's record mapper
    public class CompositeCommandPublicationRecordMapper : InMemoryRecordMapper<CommandPublicationRecord>
    {
    }
}