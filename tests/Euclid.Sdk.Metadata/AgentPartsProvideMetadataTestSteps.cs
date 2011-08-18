using System;
using System.Linq;
using Euclid.Framework.AgentMetadata;
using Euclid.Framework.AgentMetadata.Extensions;
using Euclid.Sdk.FakeAgent.Commands;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Euclid.Sdk.Metadata
{
	[Binding]
	public class AgentPartsProvideMetadataTestSteps
	{
		private IAgentMetadata _agent;
		private string _format;

		[Then(@"has been independently validated")]
		public void IndependentlyValidate()
		{
			testRepresentation(_agent.GetRepresentation(_format));
			testRepresentation(_agent.GetBasicRepresentation(_format));

			validateFormattersInCollectionOutput(_agent.Commands);
			validateFormattersInCollectionOutput(_agent.ReadModels);
			validateFormattersInCollectionOutput(_agent.Queries);
		}

		[Then(@"can be represented as (.*)")]
		public void ItCanBeRepresentedAs(string contentType)
		{
			testThatFormatterSupportsContentType(contentType, _agent);

			validateFormattersInCollectionSupportContentType(contentType, _agent.Commands);
			validateFormattersInCollectionSupportContentType(contentType, _agent.Queries);
			validateFormattersInCollectionSupportContentType(contentType, _agent.ReadModels);
		}

		[When(@"metadata is requested as (.*)")]
		public void MetadataIsRequested(string format)
		{
			if (string.IsNullOrEmpty(format))
			{
				Assert.Fail("format not specified");
			}

			_format = format;

			Console.WriteLine("Testing if '{0}' supports format '{1}'", _agent.GetType().Name, _format);
		}

		[Given("an agent")]
		public void ValidAgentMetadata()
		{
			_agent = typeof (FakeCommand).Assembly.GetAgentMetadata();
		}

		private void testRepresentation(string representation)
		{
			Assert.False(string.IsNullOrEmpty(representation));

			Console.WriteLine(representation);
		}

		private void testThatFormatterSupportsContentType(string contentType, IMetadataFormatter formatter)
		{
			Assert.AreEqual(contentType, formatter.GetContentType(_format));

			Assert.True(formatter.GetFormats(contentType).Contains(_format));
		}

		private void validateFormattersInCollectionOutput(IPartCollection part)
		{
			testRepresentation(part.GetFormatter().GetRepresentation(_format));

			foreach (var typeMetadata in part.Collection)
			{
				testRepresentation(typeMetadata.GetFormatter().GetRepresentation(_format));
			}
		}

		private void validateFormattersInCollectionSupportContentType(string contentType, IPartCollection part)
		{
			testThatFormatterSupportsContentType(contentType, part.GetFormatter());

			foreach (var typeMetadata in part.Collection)
			{
				testThatFormatterSupportsContentType(contentType, typeMetadata.GetFormatter());
			}
		}
	}
}