using System;
using AutoMapper;
using Euclid.Composite.MvcApplication.Models;
using Euclid.Composites.Conversion;
using Euclid.Framework.Agent.Extensions;
using Euclid.Framework.Cqrs;
using Euclid.Sdk.FakeAgent.Commands;

namespace Euclid.Composite.MvcApplication.EuclidConfiguration.TypeConverters
{
	public class InputToFakeCommand4Converter : IInputToCommandConverter
	{
		public Type CommandType
		{
			get { return typeof (FakeCommand); }
		}

		public Type InputModelType
		{
			get { return typeof (InputModelFakeCommand4); }
		}

		public ICommand Convert(ResolutionContext context)
		{
			var source = context.SourceValue as InputModelFakeCommand4;
			if (source == null)
			{
				throw new CannotCreateInputModelException(typeof (FakeCommand).GetMetadata().Name);
			}

			var command = Activator.CreateInstance<FakeCommand>();

			return command;
		}
	}
}