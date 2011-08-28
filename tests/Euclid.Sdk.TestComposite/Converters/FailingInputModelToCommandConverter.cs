using System;
using AutoMapper;
using Euclid.Composites.Conversion;
using Euclid.Framework.Cqrs;
using Euclid.Sdk.TestAgent.Commands;
using Euclid.Sdk.TestComposite.Models;

namespace Euclid.Sdk.TestComposite.Converters
{
    public class FailingInputModelToCommandConverter : IInputToCommandConverter
    {
        public ICommand Convert(ResolutionContext context)
        {
            var source = context.SourceValue as FailingInputModel;
            var command = context.DestinationValue as FailingCommand;

            if (source == null || command == null)
            {
                throw new CannotCreateInputModelException(InputModelType.Name);
            }

            return command;
        }

        public Type CommandType { get { return typeof(FailingCommand); } }

        public Type InputModelType { get { return typeof(FailingInputModel); } }
    }
}