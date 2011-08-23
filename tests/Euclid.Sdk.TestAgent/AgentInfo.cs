using Euclid.Framework.Agent;
using Euclid.Sdk.TestAgent.Commands;
using Euclid.Sdk.TestAgent.Processors;
using Euclid.Sdk.TestAgent.Queries;
using Euclid.Sdk.TestAgent.ReadModels;

[assembly: AgentSystemName(Value = "SDKTests.FakeAgent")]
[assembly: AgentName(Value = "Fake Agent")]
[assembly: LocationOfCommands(NamespaceOfType = typeof (TestCommand))]
[assembly: LocationOfQueries(NamespaceOfType = typeof (TestQuery))]
[assembly: LocationOfProcessors(NamespaceOfType = typeof (TestCommandProcessor))]
[assembly: LocationOfReadModels(NamespaceOfType = typeof (TestReadModel))]