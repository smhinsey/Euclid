using System;
using AutoMapper;
using Euclid.Agent.Extensions;
using Euclid.Composite.MvcApplication.Models;
using Euclid.Composites.Conversion;
using Euclid.Framework.Cqrs;
using Euclid.Framework.TestingFakes.Cqrs;

namespace Euclid.Composite.MvcApplication.EuclidConfiguration.TypeConverters
{
    public class InputToFakeCommand4Converter : IInputToCommandConverter
    {
        public ICommand Convert(ResolutionContext context)
        {
            var source = context.SourceValue as InputModelFakeCommand4;
            if (source == null)
            {
                throw new CannotCreateInputModelException(typeof(FakeCommand4).GetMetadata().Name);
            }

            var command = Activator.CreateInstance<FakeCommand4>();

            command.YourBirthday = source.BirthDay;

            command.PasswordHash = string.Format("hashed: {0}", source.Password);

            command.PasswordSalt = string.Format("salted: {0}", source.Password);

            return command;
        }

        public Type InputModelType { get { return typeof(InputModelFakeCommand4); } }
        public Type CommandType { get { return typeof(FakeCommand4); } }
    }
}