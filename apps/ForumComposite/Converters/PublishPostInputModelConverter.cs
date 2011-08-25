using System;
using AutoMapper;
using Euclid.Composites.Conversion;
using Euclid.Framework.Cqrs;
using ForumAgent.Commands;
using ForumComposite.Models;

namespace ForumComposite.Converters
{
	public class PublishPostInputModelConverter : IInputToCommandConverter
	{
		public Type CommandType
		{
			get { return typeof(PublishPost); }
		}

		public Type InputModelType
		{
			get { return typeof(PublishPostInputModel); }
		}

		public ICommand Convert(ResolutionContext context)
		{
			var source = context.SourceValue as PublishPostInputModel;
			if (source == null)
			{
				throw new CannotCreateInputModelException(InputModelType.Name);
			}

			var command = Activator.CreateInstance<PublishPost>();

			command.AuthorIdentifier = Guid.Empty;
			command.Body = source.Body;
			command.CategoryIdentifier = Guid.Empty;
			command.Created = DateTime.Now;
			command.CreatedBy = Guid.Empty;
			command.Identifier = Guid.NewGuid();
			command.Title = source.Title;

			return command;
		} 
	}
}