using System;
using AutoMapper;
using Euclid.Composites.Conversion;
using Euclid.Framework.Cqrs;
using ForumAgent.Commands;
using ForumComposite.Models;

namespace ForumComposite.Converters
{
	public class CommentOnPostInputModelConverter : IInputToCommandConverter
	{
		public Type CommandType
		{
			get { return typeof(CommentOnPost); }
		}

		public Type InputModelType
		{
			get { return typeof(CommentOnPostInputModel); }
		}

		public ICommand Convert(ResolutionContext context)
		{
			var model = context.SourceValue as CommentOnPostInputModel;
			var command = context.DestinationValue as CommentOnPost;

			if (model == null || command == null)
			{
				throw new CannotCreateInputModelException(InputModelType.Name);
			}

			command.AuthorIdentifier = Guid.Empty;
			command.Body = model.Body;
			command.Created = DateTime.Now;
			command.CreatedBy = Guid.Empty;
			command.Identifier = Guid.NewGuid();
			command.Title = model.Title;

			return command;
		} 
	}
}