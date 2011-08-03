using System;
using Euclid.Composites.Maps;
using Euclid.Composites.Models;
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

        public object Map(object source)
        {
            return Map(source as IEuclidMetdata);
        }

        public IInputModel Map(IEuclidMetdata commandMetadata)
        {
            return new Mock<IInputModel>().Object;
        }
    }
}