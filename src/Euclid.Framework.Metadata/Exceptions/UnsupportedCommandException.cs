using System;

namespace Euclid.Framework.Metadata.Exceptions
{
    public class UnsupportedCommandException : Exception
    {
        public UnsupportedCommandException(string agentSystemName, Type commandType)
            : base(string.Format("The agent {0} does not support the command {1}", agentSystemName, commandType.FullName))
        {}

        public UnsupportedCommandException(string agentSystemName, string commandNamespace, string commandName)
            : base(string.Format("The agent {0} does not support the command {1}.{2}", agentSystemName, commandNamespace, commandName))
        { }

        public UnsupportedCommandException(string commandName)
            : base(string.Format("The command {0} is unsupported", commandName))
        {
        }
    }
}