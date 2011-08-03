using System;

namespace Euclid.Composites
{
    public class InvalidCompositeApplicationStateException : Exception
    {
        private readonly CompositeApplicationState _applicationState;
        private readonly CompositeApplicationState _expectedState;

        public InvalidCompositeApplicationStateException(CompositeApplicationState applicationState,
                                                         CompositeApplicationState expectedState) : base(string.Format("The composite application state was {0} but {1} was expected", applicationState, expectedState))
        {
            _applicationState = applicationState;
            _expectedState = expectedState;
        }
    }
}