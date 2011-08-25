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
			var model = context.SourceValue as PublishPostInputModel;
			var command = context.DestinationValue as PublishPost;

			if (model == null || command == null)
			{
				throw new CannotCreateInputModelException(InputModelType.Name);
			}

			command.AuthorIdentifier = Guid.Empty;
			command.Body = model.Body;
			command.CategoryIdentifier = Guid.Empty;
			command.Created = DateTime.Now;
			command.CreatedBy = Guid.Empty;
			command.Identifier = Guid.NewGuid();
			command.Title = model.Title;

			return command;
		} 
	}
}