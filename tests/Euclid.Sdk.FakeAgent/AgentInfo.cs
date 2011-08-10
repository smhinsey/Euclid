using Euclid.Framework.Metadata.Attributes;
using Euclid.Sdk.FakeAgent.Commands;

[assembly: AgentSystemName(Value = "FakeAgent")]
[assembly: AgentName(Value = "Fake Agent")]

// hardcode agent namepsace

[assembly: LocationOfCommands(NamespaceOfType = typeof (FakeCommand))]

// specify namespace by type

[assembly: LocationOfQueries(Namespace = "FakeAgent.Queries")]

// explicitly set namespace

[assembly: LocationOfProcessors(Namespace = "FakeAgent.Processors")]