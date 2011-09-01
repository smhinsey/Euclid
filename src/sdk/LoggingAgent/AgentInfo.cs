using Euclid.Framework.Agent;
using LoggingAgent.Queries;
using LoggingAgent.ReadModels;

[assembly: AgentSystemName(Value = "Euclid.CompositeInspectorAgent")]
[assembly: AgentName(Value = "Composite Inspector Agent")]
[assembly: AgentDescription(Value = "Supports the operation of the Composite Inspector.")]
[assembly: LocationOfCommands(Namespace = "")]
[assembly: LocationOfQueries(NamespaceOfType = typeof(LogQueries))]
[assembly: LocationOfProcessors(Namespace = "")]
[assembly: LocationOfReadModels(NamespaceOfType = typeof(LogEntry))]