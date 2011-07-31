using System;
using Euclid.Composites.InputModel;
using Euclid.Composites.Mvc.Maps;
using Euclid.Framework.Metadata;
using Moq;

namespace Euclid.Composites.UnitTests
{
    public class FakeMap : IMapper<IEuclidMetdata, IInputModel>
    {
        public Type Source
        {
            get { return typeof(IEuclidMetdata); }
        }

        public Type Destination { get { return typeof (IInputModel); } }


        public IInputModel Map(IEuclidMetdata commandMetadata)
        {
            return new Mock<IInputModel>().Object;
        }
    }
}