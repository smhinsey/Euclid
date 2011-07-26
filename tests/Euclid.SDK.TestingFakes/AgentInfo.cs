using Euclid.Agent;
using Euclid.SDK.TestingFakes.Composites;

[assembly: AgentName(Value = "Fake Agent")]

[assembly: AgentSystemName(Value = "Fake")]

[assembly: AgentScheme(Value = "Euclid.SDK.TestingFakes.Agent")]

// hardcode agent namepsace
[assembly: LocationOfCommands(NamespaceOfType = typeof(FakeCommand))]

// specify namespace by type
[assembly: LocationOfQueries(Namespace = "FakeAgent.Queries")]

// explicitly set namespace
[assembly: LocationOfProcessors(Namespace = "FakeAgent.Processors")]
