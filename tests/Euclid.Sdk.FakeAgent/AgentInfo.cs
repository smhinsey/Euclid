using Euclid.Agent.Attributes;
using Euclid.Sdk.FakeAgent.Commands;
using Euclid.Sdk.FakeAgent.Processors;
using Euclid.Sdk.FakeAgent.Queries;

[assembly: AgentSystemName(Value = "FakeAgent")]
[assembly: AgentName(Value = "Fake Agent")]
[assembly: LocationOfCommands(NamespaceOfType = typeof (FakeCommand))]
[assembly: LocationOfQueries(NamespaceOfType = typeof (FakeQuery))]
[assembly: LocationOfProcessors(NamespaceOfType = typeof (FakeCommandProcessor))]
[assembly: LocationOfReadModels(NamespaceOfType = typeof (FakeReadModel))]