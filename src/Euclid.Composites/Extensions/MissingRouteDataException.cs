using System;

namespace Euclid.Composites.Extensions
{
    public class MissingRouteDataException : Exception
    {
        public MissingRouteDataException(string name)
        {
            throw new NotImplementedException(string.Format("There was no route data for key: {0}", name));
        }
    }
}