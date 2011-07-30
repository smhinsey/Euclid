using Euclid.Agent;
using Euclid.SDK.TestingFakes.Composites;

[assembly: AgentName(Value = "Fake Agent")]
[assembly: AgentSystemName(Value = "Fake")]

// hardcode agent namepsace

[assembly: LocationOfCommands(NamespaceOfType = typeof (FakeCommand))]

// specify namespace by type

[assembly: LocationOfQueries(Namespace = "FakeAgent.Queries")]

// explicitly set namespace

[assembly: LocationOfProcessors(Namespace = "FakeAgent.Processors")]