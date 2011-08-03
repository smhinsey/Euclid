using System;

namespace Euclid.Framework.Metadata.Exceptions
{
    internal class PartImplementationMismatchException : Exception
    {
        public PartImplementationMismatchException(Type agentPartImplementationType, string expectedImplementationTypeName) : base(string.Format("The agent part {0} does not implement the expected type {1}", agentPartImplementationType.FullName, expectedImplementationTypeName))
        {
        }
    }
}