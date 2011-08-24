using Euclid.Framework.Agent;
using ForumAgent.Commands;
using ForumAgent.Processors;
using ForumAgent.Queries;
using ForumAgent.ReadModels;

[assembly: AgentName(Value = "The Forum Agent supports posting, commenting, voting, moderation, and basic user profiles.")]
[assembly: AgentSystemName(Value = "ForumAgent")]
[assembly: LocationOfCommands(NamespaceOfType = typeof(CommentOnPost))]
[assembly: LocationOfQueries(NamespaceOfType = typeof(PostQueries))]
[assembly: LocationOfProcessors(NamespaceOfType = typeof(PublishPostProcessor))]
[assembly: LocationOfReadModels(NamespaceOfType = typeof(Post))]