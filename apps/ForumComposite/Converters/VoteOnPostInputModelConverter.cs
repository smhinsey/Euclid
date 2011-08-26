using System;
using AutoMapper;
using Euclid.Composites.Conversion;
using Euclid.Framework.Cqrs;
using ForumAgent.Commands;
using ForumComposite.Models;

namespace ForumComposite.Converters
{
	public class VoteOnPostInputModelConverter : IInputToCommandConverter
	{
		public Type CommandType
		{
			get { return typeof(VoteOnPost); }
		}

		public Type InputModelType
		{
			get { return typeof(VoteOnPostInputModel); }
		}

		public ICommand Convert(ResolutionContext context)
		{
			var model = context.SourceValue as VoteOnPostInputModel;
			var command = context.DestinationValue as VoteOnPost;

			if (model == null || command == null)
			{
				throw new CannotCreateInputModelException(InputModelType.Name);
			}

			command.Created = DateTime.Now;
			command.CreatedBy = Guid.Empty;
			command.Identifier = Guid.NewGuid();
			command.AuthorIdentifier = model.AuthorIdentifier;
			command.PostIdentifier = model.PostIdentifier;
			command.VoteUp = model.VoteUp;

			return command;
		} 
	}
}