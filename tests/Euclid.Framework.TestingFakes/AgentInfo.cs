using Euclid.Agent.Attributes;
using Euclid.Framework.TestingFakes.Cqrs;

[assembly: AgentSystemName(Value = "Euclid.Framework.TestingFakeAgent")]
[assembly: AgentName(Value = "Testing Fake Agent")]

// hardcode agent namepsace

[assembly: LocationOfCommands(NamespaceOfType = typeof (FakeCommand))]

// specify namespace by type

[assembly: LocationOfQueries(Namespace = "FakeAgent.Queries")]

// explicitly set namespace

[assembly: LocationOfProcessors(Namespace = "FakeAgent.Processors")]