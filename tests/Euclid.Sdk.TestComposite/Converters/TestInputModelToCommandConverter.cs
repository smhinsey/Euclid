using System;
using AutoMapper;
using Euclid.Composites.Conversion;
using Euclid.Framework.Cqrs;
using Euclid.Sdk.TestAgent.Commands;
using Euclid.Sdk.TestComposite.Models;

namespace Euclid.Sdk.TestComposite.Converters
{
    public class TestInputModelToCommandConverter : IInputToCommandConverter
    {
        public ICommand Convert(ResolutionContext context)
        {
            var source = context.SourceValue as TestInputModel;
            if (source == null)
            {
                throw new CannotCreateInputModelException(InputModelType.Name);
            }

            var command = Activator.CreateInstance<TestCommand>();

            command.Number = source.Number;

            return command;
        }

        public Type CommandType
        {
            get { return typeof(TestCommand); }
        }

        public Type InputModelType
        {
            get { return typeof(TestInputModel); }
        }
    }
}