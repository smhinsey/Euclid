using Euclid.Framework.Agent;
using ForumAgent.Commands;
using ForumAgent.Processors;
using ForumAgent.Queries;
using ForumAgent.ReadModels;

[assembly: AgentName(Value = "Forum Agent")]
[assembly: AgentSystemName(Value = "SocialRally.Forum")]
[assembly: LocationOfCommands(NamespaceOfType = typeof (CommentOnPost))]
[assembly: LocationOfQueries(NamespaceOfType = typeof (PostQueries))]
[assembly: LocationOfProcessors(NamespaceOfType = typeof (PublishPostProcessor))]
[assembly: LocationOfReadModels(NamespaceOfType = typeof (Category))]