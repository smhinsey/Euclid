
D:\Projects\Euclid\platform>@git.exe %*
using System;
using System.Reflection;

namespace Euclid.Agent
{
    public class AssemblyNotAgentException : Exception
    {
        public AssemblyNotAgentException(Assembly assembly, Type expectedAttribute)
            : base(string.Format("The assembly {0} is not an agent.  The required metadata was not found {1}", assembly.FullName, expectedAttribute.Name))
        {
        }

        public AssemblyNotAgentException(Assembly assembly) 
            : base(string.Format("The assembly {0} is not an agent", assembly.FullName))
        {}
    }
}
D:\Projects\Euclid\platform>@set ErrorLevel=%ErrorLevel%

D:\Projects\Euclid\platform>@rem Restore the original console codepage.

D:\Projects\Euclid\platform>@chcp %cp_oem% > nul < nul
