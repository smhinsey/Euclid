using Euclid.Common.Storage;
using Euclid.Framework.Cqrs;

namespace Euclid.Composite.MvcApplication.EuclidConfiguration
{
    // jt: this is an example of the composite app configuring it's record mapper
    public class CompositeCommandPublicationRecordMapper : InMemoryRecordMapper<CommandPublicationRecord>
    {
    }
}