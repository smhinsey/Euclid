using System;

namespace Euclid.Composites.Mvc.Maps
{
    public class CannotCreateInputModelException : Exception
    {
        public CannotCreateInputModelException(Type inputModelImplementationType)
            : base(string.Format("Unable to create an input model of type {0}", inputModelImplementationType))
        {
        }
    }
}
