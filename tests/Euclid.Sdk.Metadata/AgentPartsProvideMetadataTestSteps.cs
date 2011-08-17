using System;
using System.Linq;
using Euclid.Framework.Agent.Extensions;
using Euclid.Framework.Agent.Metadata;
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
			TestRepresentation(_agent.GetRepresentation(_format));
			TestRepresentation(_agent.GetBasicRepresentation(_format));

			ValidateFormattersInCollectionOutput(_agent.Commands);
			ValidateFormattersInCollectionOutput(_agent.ReadModels);
			ValidateFormattersInCollectionOutput(_agent.Queries);
		}

		[Then(@"can be represented as (.*)")]
		public void ItCanBeRepresentedAs(string contentType)
		{
			TestThatFormatterSupportsContentType(contentType, _agent);

			ValidateFormattersInCollectionSupportContentType(contentType, _agent.Commands);
			ValidateFormattersInCollectionSupportContentType(contentType, _agent.Queries);
			ValidateFormattersInCollectionSupportContentType(contentType, _agent.ReadModels);
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

		private void TestRepresentation(string representation)
		{
			Assert.False(string.IsNullOrEmpty(representation));

			Console.WriteLine(representation);
		}

		private void TestThatFormatterSupportsContentType(string contentType, IMetadataFormatter formatter)
		{
			Assert.AreEqual(contentType, formatter.GetContentType(_format));

			Assert.True(formatter.GetFormats(contentType).Contains(_format));
		}

		private void ValidateFormattersInCollectionOutput(IPartCollection part)
		{
			TestRepresentation(part.GetFormatter().GetRepresentation(_format));
			foreach (var typeMetadata in part.Collection)
			{
				TestRepresentation(typeMetadata.GetFormatter().GetRepresentation(_format));
			}
		}

		private void ValidateFormattersInCollectionSupportContentType(string contentType, IPartCollection part)
		{
			TestThatFormatterSupportsContentType(contentType, part.GetFormatter());
			foreach (var typeMetadata in part.Collection)
			{
				TestThatFormatterSupportsContentType(contentType, typeMetadata.GetFormatter());
			}
		}
	}
}