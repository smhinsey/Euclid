using System.Data;
using Euclid.Common.Pipeline;
using Euclid.Composites.InputModel;
using Euclid.Framework.Cqrs;
using Euclid.Framework.Cqrs.Metadata;

namespace Euclid.Composites.Mvc.MappingPipeline
{
    public class GetCommandStep : GetMapStepBase, IPipelineStep<MapCommandPipelineData>
    {
        private readonly MapperRegistry _registry;
        public PipelinePriority Priority { get; set; }

        public GetCommandStep(MapperRegistry registry)
        {
            _registry = registry;
        }

        public MapCommandPipelineData Execute(MapCommandPipelineData data)
        {
            GuardAgainstNull(_registry);

            GuardAgainstNull(data.InputModel);

            var mapper = GetMapperFromRegistry();
            
            data.Command = mapper.Map(data.InputModel);

            return data;
        }

        private IMapper<IInputModel, ICommand> GetMapperFromRegistry()
        {
            return _registry.Get<IInputModel, ICommand>();
        }
    }
}