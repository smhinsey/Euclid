using System;
using Euclid.Composites.InputModel;
using Euclid.Framework.Metadata;

namespace Euclid.Composites.Mvc.Maps
{
    public class DefaultComponentMetadataToInputModelMap : IMapper<IEuclidMetdata, IInputModel>
    {
        public Type Source { get { return typeof(IEuclidMetdata); } }
        public Type Destination { get { return typeof(IInputModel); } }

        public IInputModel Map(IEuclidMetdata commandMetadata)
        {
            var inputModel = Activator.CreateInstance(typeof(Models.InputModel)) as IInputModel;

            if (inputModel == null)
            {
                throw new CannotCreateInputModelException(typeof(Models.InputModel));
            }

            inputModel.CommandType = commandMetadata.Type;
            inputModel.AgentSystemName = string.Empty;


//            commandMetadata.Properties.ToList().ForEach(md => inputModel.Properties.Add(new EuclidMetadata(md.Type) { }));

            return inputModel;
        }
    }
}