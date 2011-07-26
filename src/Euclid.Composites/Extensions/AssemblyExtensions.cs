using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Euclid.Agent;
using Euclid.Composites.Metadata;
using Euclid.Framework.Cqrs.Metadata;

namespace Euclid.Composites.Extensions
{
	public static class AssemblyExtensions
	{
		public static bool ContainsAgent(this Assembly assembly)
		{
			var agentAttributeTypes = new List<Type>
			                          	{
			                          		typeof (AgentNameAttribute),
			                          		typeof (AgentSchemeAttribute),
			                          		typeof (AgentSystemNameAttribute),
			                          		typeof (LocationOfCommandsAttribute),
			                          		typeof (LocationOfQueriesAttribute),
			                          		typeof (LocationOfProcessorsAttribute)
			                          	};

			var attributes = assembly.GetCustomAttributes(false).Where(attr => attr.GetType().GetInterface(typeof (IAgentAttribute).Name) != null).Select(x => x.GetType()).ToList();

			return attributes.Intersect(agentAttributeTypes).Count() == attributes.Count();
		}

		public static IAgentMetadata GetAgentMetadata(this Assembly assembly)
		{
			IAgentMetadata metadata = null;

			try
			{
				metadata = new AgentMetadata
				           	{
				           		FriendlyName = assembly.GetAttributeValue<AgentNameAttribute>().Value,
				           		Scheme = assembly.GetAttributeValue<AgentSchemeAttribute>().Value,
				           		SystemName = assembly.GetAttributeValue<AgentSystemNameAttribute>().Value,
				           		CommandNamespace = assembly.GetAttributeValue<LocationOfCommandsAttribute>().Namespace,
				           		QueryNamespace = assembly.GetAttributeValue<LocationOfQueriesAttribute>().Namespace,
				           		CommandProcessorNamespace = assembly.GetAttributeValue<LocationOfProcessorsAttribute>().Namespace
				           	};
			}
			catch (AssemblyNotAgentException)
			{
				// REMARK eat this exception
			}

			return metadata;
		}

		public static T GetAttributeValue<T>(this Assembly assembly) where T : Attribute
		{
			var attributes = assembly.GetCustomAttributes(typeof (T), false);

			if (attributes.Count() == 0)
			{
				throw new AssemblyNotAgentException(assembly, typeof (T));
			}

			var attribute = attributes[0] as T;

			if (attribute == null)
			{
				throw new AssemblyNotAgentException(assembly, typeof (T));
			}

			return attribute;
		}
	}
}