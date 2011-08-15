using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Euclid.Agent.Extensions;
using Euclid.Framework.Agent.Metadata;
using Euclid.Sdk.FakeAgent.Commands;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Euclid.Sdk.Metadata
{
    [Binding]
    public class EuclidMetadataProviderTestSteps
    {
        private string _format;
        private IAgentMetadata _agent;

        [Given("an agent")]
        public void ValidAgentMetadata()
        {
            _agent = typeof(FakeCommand).Assembly.GetAgentMetadata();
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

        [Then(@"can be represented as (.*)")]
        public void ItCanBeRepresentedAs(string contentType)
        {
            AssertValidProviders(contentType, _agent);

            AssertValidProviders(contentType, _agent.Commands);
            foreach(var c in _agent.Commands)
            {
                AssertValidProviders(contentType, c.GetMetadataFormatter("command"));
            }

            AssertValidProviders(contentType, _agent.ReadModels);
            foreach(var m in _agent.ReadModels)
            {
                AssertValidProviders(contentType, m.GetMetadataFormatter("readmodel"));
            }

            AssertValidProviders(contentType, _agent.Queries);
            foreach (var q in _agent.Queries)
            {
                AssertValidProviders(contentType, q.GetMetadataFormatter("query"));
            }
       }

         [Then(@"has been independently validated")]
         public void IndependentlyValidate()
         {
             ValidateProviderOutput(_agent.GetRepresentation(_format));

             ValidateProviderOutput(_agent.GetBasicMetadata(_format));

             ValidateProviderOutput(_agent.Commands.GetRepresentation(_format));
             foreach (var c in _agent.Commands)
             {
                 ValidateProviderOutput(c.GetMetadataFormatter("command").GetRepresentation(_format));
             }

             ValidateProviderOutput(_agent.ReadModels.GetRepresentation(_format));
             foreach (var m in _agent.ReadModels)
             {
                 ValidateProviderOutput(m.GetMetadataFormatter("readmodel").GetRepresentation(_format));
             }

             ValidateProviderOutput(_agent.Queries.GetRepresentation(_format));
             foreach (var q in _agent.Queries)
             {
                 ValidateProviderOutput(q.GetMetadataFormatter("query").GetRepresentation(_format));
             }
         }

        private void AssertValidProviders(string contentType, IFormattedMetadataProvider provider)
        {
            Assert.AreEqual(contentType, provider.GetContentType(_format));

            Assert.True(provider.GetFormats(contentType).Contains(_format));
        }

        private void ValidateProviderOutput(string representation)
        {
            Assert.False(string.IsNullOrEmpty(representation));

            Console.WriteLine(representation);
        }
    }
}