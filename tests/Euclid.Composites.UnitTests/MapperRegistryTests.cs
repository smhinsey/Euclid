using Euclid.Composites.Maps;
using Moq;
using NUnit.Framework;

namespace Euclid.Composites.UnitTests
{
    [TestFixture]
    public class MapperRegistryTests
    {
        [Test]
        public void TestRegistry()
        {
            //Arrange
            var mockMap = new Mock<IMapper<int, string>>();
            mockMap.Setup(m => m.Source).Returns(typeof (int));
            mockMap.Setup(m => m.Destination).Returns(typeof (string));

            var registry = new MapperRegistry();

            registry.Add(mockMap.Object);

            var map = registry.Get<int, string>();
            Assert.NotNull(map);
        }
    }
}