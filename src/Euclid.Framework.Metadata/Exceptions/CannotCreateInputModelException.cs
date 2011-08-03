using System;

namespace Euclid.Framework.Metadata.Exceptions
{
    public class CannotCreateInputModelException : Exception
    {
        public CannotCreateInputModelException(Type inputModelImplementationType)
            : base(string.Format("Unable to create an input model of type {0}", inputModelImplementationType))
        {
        }
    }
}
