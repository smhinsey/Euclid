using System;
using System.Linq;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Euclid.Sdk.Metadata
{
	[Binding]
	public class CommonWhenThenAndSteps : PropertiesUsedInTests
	{
		[Then(@"has been independently validated")]
		public void IndependentlyValidate()
		{
			Assert.NotNull(this.Formatter);

			Assert.False(string.IsNullOrEmpty(this.Format));

			var representation = this.Formatter.GetRepresentation(this.Format);

			Assert.False(string.IsNullOrEmpty(representation));

			Console.WriteLine(representation);
		}

		[Then(@"it can be represented as (.*)")]
		public void ItCanBeRepresentedAs(string contentType)
		{
			Assert.NotNull(this.Formatter);

			Assert.False(string.IsNullOrEmpty(this.Format));

			Assert.False(string.IsNullOrEmpty(contentType));

			Assert.AreEqual(contentType, this.Formatter.GetContentType(this.Format));

			Assert.True(this.Formatter.GetFormats(contentType).Contains(this.Format));
		}

		[When(@"metadata is requested as (.*)")]
		public void MetadataIsRequested(string format)
		{
			Assert.False(string.IsNullOrEmpty(format));

			this.Format = format;
		}
	}
}