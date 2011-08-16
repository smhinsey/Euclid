using System;

namespace Euclid.Framework.Agent.Metadata
{
    public class InvalidAgentPartTypeSpecifiedException : Exception
    {
        public InvalidAgentPartTypeSpecifiedException(string partType) :base(partType)
        {
        }
    }
}