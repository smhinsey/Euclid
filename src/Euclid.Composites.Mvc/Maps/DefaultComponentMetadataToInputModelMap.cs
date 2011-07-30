using System;
using System.Linq;
using Euclid.Composites.InputModel;
using Euclid.Composites.Mvc.MappingPipeline;
using Euclid.Framework.Cqrs.Metadata;

namespace Euclid.Composites.Mvc.Maps
{
    public class DefaultComponentMetadataToInputModelMap : IMapper<ICommandMetadata, IInputModel>
    {
        public Type Source { get { return typeof(ICommandMetadata); } }
        public Type Destination { get { return typeof(IInputModel); } }

        public IInputModel Map(ICommandMetadata commandMetadata)
        {
            var inputModel = Activator.CreateInstance(typeof(Models.InputModel)) as IInputModel;

            if (inputModel == null)
            {
                throw new CannotCreateInputModelException(typeof(Models.InputModel));
            }

            inputModel.CommandType = commandMetadata.CommandType;
            inputModel.AgentSystemName = string.Empty;
            commandMetadata.Properties.ToList().ForEach(md => inputModel.Properties.Add(md));

            return inputModel;
        }
    }
}