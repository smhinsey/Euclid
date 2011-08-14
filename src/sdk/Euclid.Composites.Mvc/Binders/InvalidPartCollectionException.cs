using System;

namespace Euclid.Composites.Mvc.Binders
{
    public class InvalidPartCollectionException : Exception
    {
        public InvalidPartCollectionException(string partType)
            : base(string.Format("Agents have no metadata collection named '{0}'", partType))
        {
        }
    }
}