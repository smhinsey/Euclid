using System;
using System.Collections.Generic;
using System.Reflection;
using Euclid.Agent;
using Euclid.Agent.Extensions;
using Euclid.Framework.Metadata;

namespace Euclid.Composites
{
	public abstract class DefaultCompositeApp
	{
		public readonly IList<IAgentMetadata> Agents = new List<IAgentMetadata>();
		protected CompositeApplicationState ApplicationState { get; set; }

		public void InstallAgent(Assembly assembly)
		{
			if (assembly == null)
			{
				throw new ArgumentNullException("assembly");
			}

			if (!assembly.ContainsAgent())
			{
				throw new AssemblyNotAgentException(assembly);
			}

			Agents.Add(assembly.GetAgentMetadata());
		}
	}
}