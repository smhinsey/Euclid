using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Euclid.Common.Pipeline;
using Euclid.Composites.InputModel;
using Euclid.Framework.Cqrs.Metadata;
using FluentNHibernate.Automapping;

namespace Euclid.Composites.Mvc.MappingPipeline
{
    public class GetInputModelStep : GetMapStepBase, IPipelineStep<MapCommandPipelineData>
    {
        private readonly MapperRegistry _registry;
        public PipelinePriority Priority { get; set; }

        public GetInputModelStep(MapperRegistry registry)
        {
            _registry = registry;
        }

        public MapCommandPipelineData Execute(MapCommandPipelineData data)
        {
            GuardAgainstNull(_registry);

            GuardAgainstNull(data);

            GuardAgainstNull(data.CommandMetadata);

            var mapper = GetMapperFromRegistry();
            
            data.InputModel  = mapper.Map(data.CommandMetadata);

            return data;
        }

        private IMapper<ICommandMetadata, IInputModel> GetMapperFromRegistry()
        {
            return _registry.Get<ICommandMetadata, IInputModel>();
        }

        //REMARK: Guard Clause
    }
}