using System;
using System.IO;
using System.Reflection;

namespace Euclid.Composites.Agent
{
    public class FileSystemAgentResolver : AgentResolverBase
    {
        public override Assembly GetAgent(string scheme, string systemName)
        {
            var agent = GetAssembly(scheme, systemName, Environment.CurrentDirectory) ??
                        GetAssembly(scheme, systemName, AppDomain.CurrentDomain.RelativeSearchPath) ??
                        GetAssembly(scheme, systemName, AppDomain.CurrentDomain.DynamicDirectory);
                        
            return agent;
        }

        private Assembly GetAssembly(string scheme, string systemName, string directory)
        {
            foreach (var filePath in Directory.EnumerateFiles(directory, "*.dll", SearchOption.AllDirectories))
            {
                var assembly = System.Reflection.Assembly.LoadFrom(filePath);

                if (IsAgent(assembly, scheme, systemName))
                {
                    return assembly;
                }
            }

            return null;
        }
    }
}