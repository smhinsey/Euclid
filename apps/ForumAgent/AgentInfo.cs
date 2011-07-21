using Euclid.Agent;
using ForumAgent.Queries;

[assembly: AgentName("Forum Agent")]
[assembly: LocationOfCommands(Namespace = "ForumAgent.Commands")]
[assembly: LocationOfQueries(NamespaceOfType = typeof(PostQueries))]
[assembly: LocationOfProcessors(Namespace = "ForumAgent.Processors")]
