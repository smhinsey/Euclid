using System;
using AutoMapper;
using Euclid.Framework.Cqrs;
using ForumAgent.Commands;
using ForumComposite.Models;

namespace ForumComposite.Converters
{
	public class VoteOnCommentInputModelConverter : IInputToCommandConverter
	{
		public Type CommandType
		{
			get
			{
				return typeof(VoteOnComment);
			}
		}

		public Type InputModelType
		{
			get
			{
				return typeof(VoteOnCommentInputModel);
			}
		}

		public ICommand Convert(ResolutionContext context)
		{
			var model = context.SourceValue as VoteOnCommentInputModel;
			var command = context.DestinationValue as VoteOnComment;

			if (model == null || command == null)
			{
				throw new CannotCreateInputModelException(InputModelType.Name);
			}

			command.Created = DateTime.Now;
			command.CreatedBy = Guid.Empty;
			command.Identifier = Guid.NewGuid();
			command.AuthorIdentifier = model.AuthorIdentifier;
			command.CommentIdentifier = model.CommentIdentifier;
			command.VoteUp = model.VoteUp;

			return command;
		}
	}
}