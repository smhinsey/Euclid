using System;

namespace Euclid.Framework.Metadata.Exceptions
{
    internal class CommandNotRegisteredException : Exception
    {
        public CommandNotRegisteredException(Type commandType)
            : base(
                string.Format("The command {0} is not registered with this agent's CommandCollection",
                              commandType.FullName))
        {
        }
    }
}