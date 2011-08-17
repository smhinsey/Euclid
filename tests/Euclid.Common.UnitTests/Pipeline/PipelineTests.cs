using Euclid.Common.Pipeline;
using Euclid.TestingSupport;
using Moq;
using NUnit.Framework;

namespace Euclid.Common.UnitTests.Pipeline
{
	[TestFixture]
	[Category(TestCategories.Unit)]
	public class PipelineTests
	{
		[Test]
		public void ExecutePipeline()
		{
			//Arrange
			const int expected = 2;
			var mockStep = new Mock<IPipelineStep<int>>();

			mockStep.Setup(x => x.Execute(0)).Returns(expected);

			//Act
			var pipeline = new Pipeline<int>();
			pipeline.Configure(new[] {mockStep.Object});
			var results = pipeline.Process(0);

			//Assert
			Assert.AreEqual(expected, results);
		}

		[Test]
		[ExpectedException(typeof (StepConfigurationException))]
		public void TestStepConfigurationExceptionWithListContainingNullElement()
		{
			var pipeline = new Pipeline<int>();
			pipeline.Configure(new IPipelineStep<int>[] {null});
		}

		[Test]
		[ExpectedException(typeof (StepConfigurationException))]
		public void TestStepConfigurationExceptionWithNullList()
		{
			//Act
			var pipeline = new Pipeline<int>();
			pipeline.Configure(null);
		}

		[Test]
		public void TestStepExecutionException()
		{
			var mockStep = new Mock<IPipelineStep<int>>();

			mockStep.Setup(x => x.Execute(0)).Throws(new StepExecutionException(0, typeof (int), null));

			//Act
			var pipeline = new Pipeline<int>();
			pipeline.Configure(new[] {mockStep.Object});

			Assert.Throws<StepExecutionException>(() => pipeline.Process(0));
		}
	}
}