using System;
using System.Data;
using Euclid.Composites.InputModel;
using Euclid.Framework.Cqrs.Metadata;

namespace Euclid.Composites.Mvc.MappingPipeline
{
    public class GetMapStepBase
    {
        protected void GuardAgainstNull(MapCommandPipelineData data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
        }

        protected void GuardAgainstNull(IInputModel inputModel)
        {
            if (inputModel == null)
            {
                throw new NoNullAllowedException("MapCommandPiplineData.InputModel");
            }
        }

        protected void GuardAgainstNull(MapperRegistry registry)
        {
            if (registry == null)
            {
                throw new NoRegistryConfiguredException();
            }
        }

        protected void GuardAgainstNull(ICommandMetadata commandMetadata)
        {
            if (commandMetadata == null)
            {
                throw new NoNullAllowedException("MapCommandPiplineData.CommandMetdata");
            }
        }
    }
}