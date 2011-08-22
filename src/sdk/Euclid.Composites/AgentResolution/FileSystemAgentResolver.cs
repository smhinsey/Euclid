using System;
using System.IO;
using System.Reflection;

namespace Euclid.Composites.AgentResolution
{
	public class FileSystemAgentResolver : AgentResolverBase
	{
		public override Assembly GetAgent(string systemName)
		{
			var agent = this.GetAssembly(systemName, Environment.CurrentDirectory)
			            ??
			            this.GetAssembly(systemName, AppDomain.CurrentDomain.RelativeSearchPath)
			            ?? this.GetAssembly(systemName, AppDomain.CurrentDomain.DynamicDirectory);

			return agent;
		}

		private Assembly GetAssembly(string systemName, string directory)
		{
			foreach (var filePath in Directory.EnumerateFiles(directory, "*.dll", SearchOption.AllDirectories))
			{
				var assembly = Assembly.LoadFrom(filePath);

				if (this.IsAgent(assembly, systemName))
				{
					return assembly;
				}
			}

			return null;
		}
	}
}