using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Euclid.Framework.Cqrs;
using Euclid.Framework.Metadata;
using Euclid.Framework.Metadata.Attributes;

namespace Euclid.Agent.Extensions
{
    public static class AssemblyExtensions
    {
        public static bool ContainsAgent(this Assembly assembly)
        {
            var agentAttributeTypes = new List<Type>
                                          {
                                              typeof (AgentNameAttribute),
                                              typeof (AgentSystemNameAttribute),
                                              typeof (LocationOfCommandsAttribute),
                                              typeof (LocationOfQueriesAttribute),
                                              typeof (LocationOfProcessorsAttribute)
                                          };

            var attributes = assembly.GetCustomAttributes(false).Where(attr=>attr.GetType().GetInterface(typeof(IAgentAttribute).Name)!= null).Select(x=>x.GetType()).ToList();

            return attributes.Intersect(agentAttributeTypes).Count() == agentAttributeTypes.Count();
        }

        public static IAgentMetadata GetAgentMetadata(this Assembly assembly)
        {
            return new AgentMetadata(assembly);
        }

        internal static IEnumerable<Type> GetCommandTypes(this Assembly agent, string commandNamespace)
        {
            return agent
                    .GetTypes()
                    .Where(
                        x => x.Namespace == commandNamespace
                             && typeof (ICommand).IsAssignableFrom(x));
        }

        public static string GetAgentName(this Assembly agent)
        {
            return agent.GetAttributeValue<AgentNameAttribute>().Value;
        }

        public static string GetAgentSystemName(this Assembly agent)
        {
            return agent.GetAttributeValue<AgentSystemNameAttribute>().Value;
        }

        internal static string GetCommandNamespace(this Assembly agent)
        {
            return agent.GetAttributeValue<LocationOfCommandsAttribute>().Namespace;
        }

        internal static string GetQueryNamespace(this Assembly agent)
        {
            return agent.GetAttributeValue<LocationOfQueriesAttribute>().Namespace;
        }

        internal static string GetProcessorNamespace(this Assembly agent)
        {
            return agent.GetAttributeValue<LocationOfProcessorsAttribute>().Namespace;
        }

        private static T GetAttributeValue<T>(this Assembly assembly) where T : Attribute
        {
            var attributes = assembly.GetCustomAttributes(typeof (T), false);

            if (attributes.Count() == 0)
            {
                throw new AssemblyNotAgentException(assembly, typeof(T));                
            }

            var attribute = attributes[0] as T;

            if (attribute == null)
            {
                throw new AssemblyNotAgentException(assembly, typeof(T));
            }

            return attribute;
        }
    }
}