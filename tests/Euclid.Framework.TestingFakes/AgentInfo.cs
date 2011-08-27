using Euclid.Framework.Agent;
using Euclid.Framework.TestingFakes.Cqrs;

[assembly: AgentSystemName(Value = "Euclid.Framework.TestingFakeAgent")]
[assembly: AgentName(Value = "Testing Fake Agent")]

// hardcode agent namepsace

[assembly: LocationOfCommands(NamespaceOfType = typeof(FakeCommand))]

// specify namespace by type

[assembly: LocationOfQueries(Namespace = "FakeAgent.Queries")]

// explicitly set namespace

[assembly: LocationOfProcessors(Namespace = "FakeAgent.Processors")]
[assembly: AgentDescription(Value = "A fake agent used for testing in the Euclid.Framework namespace")]