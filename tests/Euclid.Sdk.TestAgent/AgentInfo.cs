using Euclid.Framework.Agent;
using Euclid.Sdk.TestAgent.Commands;
using Euclid.Sdk.TestAgent.Processors;
using Euclid.Sdk.TestAgent.Queries;
using Euclid.Sdk.TestAgent.ReadModels;

[assembly: AgentSystemName(Value = "SDKTests.TestAgent")]
[assembly: AgentName(Value = "Test Agent")]
[assembly: AgentDescription(Value = "An agent used for testing")]
[assembly: LocationOfCommands(NamespaceOfType = typeof (TestCommand))]
[assembly: LocationOfQueries(NamespaceOfType = typeof (TestQuery))]
[assembly: LocationOfProcessors(NamespaceOfType = typeof (TestCommandProcessor))]
[assembly: LocationOfReadModels(NamespaceOfType = typeof (TestReadModel))]
