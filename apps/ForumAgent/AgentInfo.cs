using Euclid.Agent;
using Euclid.Framework.Metadata.Attributes;
using ForumAgent.Queries;

[assembly: AgentName(Value = "Forum Agent")]
[assembly: AgentSystemName(Value = "Forum")]

// 3 examples of using the NamespaceFinder attribute class
// used to by the composite during agent registration

// hardcode agent namepsace

[assembly: LocationOfCommands(Namespace = "ForumAgent.Commands")]

// specify namespace by type

[assembly: LocationOfQueries(NamespaceOfType = typeof (PostQueries))]

// explicitly set namespace

[assembly: LocationOfProcessors(Namespace = "ForumAgent.Processors")]