using Euclid.Framework.Agent;
using LoggingAgent.Queries;
using LoggingAgent.ReadModels;

[assembly: AgentSystemName(Value = "LoggingAgent")]
[assembly: AgentName(Value = "Logging Agent")]
[assembly: AgentDescription(Value = "An agent that reads log4Net logs")]
[assembly: LocationOfCommands(Namespace = "")]
[assembly: LocationOfQueries(NamespaceOfType = typeof(LogQueries))]
[assembly: LocationOfProcessors(Namespace = "")]
[assembly: LocationOfReadModels(NamespaceOfType = typeof(LogEntry))]