using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Euclid.Agent;
using Euclid.Framework.Cqrs;

namespace Euclid.Framework.Metadata.Extensions
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

        public static IAgentMetadata GetAgentInfo(this Assembly assembly)
        {
            return new AgentMetadataService(assembly);
        }

        internal static IEnumerable<Type> GetCommandTypes(this Assembly agent, string commandNamespace)
        {
            return agent
                    .GetTypes()
                    .Where(
                        x => x.Namespace == commandNamespace
                             && typeof (ICommand).IsAssignableFrom(x));
        }

        internal static string GetAgentName(this Assembly agent)
        {
            return agent.GetAttributeValue<AgentNameAttribute>().Value;
        }

        internal static string GetAgentSystemName(this Assembly agent)
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

    public static class TypeExtensions
    {
        public static IEuclidMetdata GetMetadata(this Type type)
        {
            return new EuclidMetadata(type);
        }
    }
}