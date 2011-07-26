using System;

namespace Euclid.Composites.Mvc.Binders
{
    public class CommandTypeNotFoundException : Exception
    {
        public CommandTypeNotFoundException(string commandTypeName)
            : base(string.Format("Could not find a command of type {0} ensure you have specified an assembly qualified reference to the command", commandTypeName))
        {
        }
    }
}