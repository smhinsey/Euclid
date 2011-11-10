using Euclid.Common.TestingFakes.Messaging;
using Euclid.Common.TestingFakes.Transport;
using Euclid.TestingSupport;
using NUnit.Framework;

namespace Euclid.Common.UnitTests.Messaging
{
	[TestFixture]
	[Category(TestCategories.Unit)]
	public class MultipleMessageProcessorTests
	{
		[Test]
		public void MultipleMessagesAreRecognizedByProcessor()
		{
			var processor = new FakeMultipleMessageProcessor();

			var fakeMessage = new FakeMessage();
			var differentMessage = new DifferentFakeMessage();

			Assert.IsTrue(processor.CanProcessMessage(fakeMessage));
			Assert.IsTrue(processor.CanProcessMessage(differentMessage));
		}
	}
}