using System;
using System.Data;
using Euclid.Composites.InputModel;
using Euclid.Composites.Metadata;
using Euclid.Composites.Mvc.MappingPipeline;
using Euclid.Framework.Cqrs;
using Euclid.Framework.Cqrs.Metadata;
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

    [TestFixture]
    public class TestMappingPipelineSteps
    {
        [Test]
        public void TestMetadataToInputModelStep()
        {
            var registry = new MapperRegistry();
            registry.Add(new FakeMap());

            var map = registry.Get<ICommandMetadata, IInputModel>();
            Assert.NotNull(map);

            var data = new MapCommandPipelineData
            {
                CommandMetadata = new Mock<ICommandMetadata>().Object
            };

            var step = new GetInputModelStep(registry);
            data = step.Execute(data);

            Assert.NotNull(data.InputModel);
        }

        [Test]
        [ExpectedException(typeof(NoRegistryConfiguredException))]
        public void TestNullRegistryInStepThrowsException()
        {
            var step = new GetInputModelStep(null);

            step.Execute(null);
        }

        [Test]
        [ExpectedException(typeof(NoNullAllowedException))]
        public void TestPipelineDataContainsNullValue()
        {
            var registry = new MapperRegistry();
            registry.Add(new FakeMap());

            var map = registry.Get<ICommandMetadata, IInputModel>();
            Assert.NotNull(map);

            var data = new MapCommandPipelineData
            {
                CommandMetadata = null
            };

            var step = new GetInputModelStep(registry);
            data = step.Execute(data);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestPipelineDataaIsNullThrowsException()
        {
            var registry = new MapperRegistry();
            registry.Add(new FakeMap());

            var map = registry.Get<ICommandMetadata, IInputModel>();
            Assert.NotNull(map);

            var step = new GetInputModelStep(registry);
            var data = step.Execute(null);
        }

    }
}