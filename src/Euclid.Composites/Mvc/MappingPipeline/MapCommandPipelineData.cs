using Euclid.Composites.InputModel;
using Euclid.Framework.Cqrs;
using Euclid.Framework.Cqrs.Metadata;

namespace Euclid.Composites.Mvc.MappingPipeline
{
    public class MapCommandPipelineData
    {
        public ICommandMetadata CommandMetadata { get; set; }
        public IInputModel InputModel { get; set; }
        public ICommand Command { get; set; }
    }
}