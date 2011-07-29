
D:\Projects\Euclid\platform>@git.exe %*
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
D:\Projects\Euclid\platform>@set ErrorLevel=%ErrorLevel%

D:\Projects\Euclid\platform>@rem Restore the original console codepage.

D:\Projects\Euclid\platform>@chcp %cp_oem% > nul < nul
