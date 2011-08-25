using System;
using System.IO;
using System.Reflection;
using Euclid.Common.Logging;

namespace Euclid.Composites.AgentResolution
{
	public class FileSystemAgentResolver : AgentResolverBase, ILoggingSource
	{
		public override Assembly GetAgent(string systemName)
		{
			var agent = GetAssembly(systemName, Environment.CurrentDirectory)
			            ??
			            GetAssembly(systemName, AppDomain.CurrentDomain.RelativeSearchPath)
			            ?? GetAssembly(systemName, AppDomain.CurrentDomain.DynamicDirectory);

			return agent;
		}

		private Assembly GetAssembly(string systemName, string directory)
		{
			foreach (var filePath in Directory.EnumerateFiles(directory, "*.dll", SearchOption.AllDirectories))
			{
				try
				{
					var assembly = Assembly.LoadFrom(filePath);

					if (IsAgent(assembly, systemName))
					{
						return assembly;
					}
				}
				catch (Exception exception)
				{
					this.WriteErrorMessage(string.Format("Error attempting to load an agent from {0}", filePath), exception);
				}
			}

			return null;
		}
	}
}
