﻿using System.Collections.Generic;
using Euclid.Framework.AgentMetadata;
using Euclid.Framework.AgentMetadata.Extensions;
using Euclid.Sdk.FakeAgent.Commands;
using TechTalk.SpecFlow;

namespace Euclid.Sdk.Metadata
{
    [Binding]
    public class AgentProvidesMetadata : PropertiesUsedInTests
    {
        [Given("an agent")]
        public void AnAgent()
        {
            Agent = typeof (FakeCommand).Assembly.GetAgentMetadata();
        }

        [When("the (.*) is requested")]
        public void FormattedMetadataIsRequested(string representationType)
        {
            switch (representationType.ToLower())
            {
                case "basic":
                    Formatter = Agent.GetBasicMetadataFormatter();
                    break;
                case "full":
                    Formatter = Agent.GetMetadataFormatter();
                    break;

            }
        }
    }

    [Binding]
    public class CollectionOfAgentsProvideMetadata : PropertiesUsedInTests
    {
        private List<IAgentMetadata> _agents;

        [Given("an agent collection")]
        public void AnAgentCollection()
        {
            _agents = new List<IAgentMetadata>
                          {
                              typeof (FakeCommand).Assembly.GetAgentMetadata()
                          };
        }

        [When("the (.*) is requested from the collection")]
        public void GetCollectionFormatter(string representationType)
        {
            switch (representationType.ToLower())
            {
                case "basic":
                    Formatter = _agents.GetBasicMetadataFormatter();
                    break;
                case "full":
                    Formatter = _agents.GetFullMetadataFormatter();
                    break;
            }
        }
    }
}