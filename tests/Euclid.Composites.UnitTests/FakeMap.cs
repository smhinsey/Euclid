using System;
using Euclid.Composites.InputModel;
using Euclid.Composites.Mvc.MappingPipeline;
using Euclid.Framework.Cqrs.Metadata;
using Moq;

namespace Euclid.Composites.UnitTests
{
    public class FakeMap : IMapper<ICommandMetadata, IInputModel>
    {
        public Type Source
        {
            get { return typeof (ICommandMetadata); }
        }

        public Type Destination { get { return typeof (IInputModel); } }


        public IInputModel Map(ICommandMetadata commandMetadata)
        {
            return new Mock<IInputModel>().Object;
        }
    }
}