using System.Linq;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Euclid.Sdk.Specifications.Metadata.Composite
{
	[Binding]
	public class CompositeProvidesConfigurationMetadata : CompositeTestProperties
	{
		[Then("the result should be (.*)")]
		public void CheckErrorResult(bool expected)
		{
			Assert.NotNull(Composite);

			Assert.AreEqual(expected, IsValid);
		}

		[Then(@"the call to GetConfigurationErrors returns an enumerable list that contains (.*) items")]
		public void GetErrorDescriptions(int count)
		{
			Assert.NotNull(Composite);

			var reasons = Composite.GetConfigurationErrors();

			Assert.AreEqual(count, reasons.Count());
		}

		[When("I call IsValid")]
		public void RetrieveConfigurationErrors()
		{
			Assert.NotNull(Composite);

			IsValid = Composite.IsValid();
		}
	}
}
