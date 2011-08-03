using System;

namespace Euclid.Composites
{
    public class InvalidCompositeApplicationStateException : Exception
    {
        private readonly CompositeApplicationState _applicationState;
        private readonly CompositeApplicationState _expectedState;

        public InvalidCompositeApplicationStateException(CompositeApplicationState applicationState,
                                                         CompositeApplicationState expectedState)
        {
            _applicationState = applicationState;
            _expectedState = expectedState;
        }
    }
}