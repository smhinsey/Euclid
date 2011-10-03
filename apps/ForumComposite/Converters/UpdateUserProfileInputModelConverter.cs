using System;
using AutoMapper;
using Euclid.Framework.Cqrs;
using ForumAgent.Commands;
using ForumComposite.Models;

namespace ForumComposite.Converters
{
	public class UpdateUserProfileInputModelConverter : IInputToCommandConverter
	{
		public Type CommandType
		{
			get
			{
				return typeof(UpdateUserProfile);
			}
		}

		public Type InputModelType
		{
			get
			{
				return typeof(UpdateUserProfileInputModel);
			}
		}

		public ICommand Convert(ResolutionContext context)
		{
			var model = context.SourceValue as UpdateUserProfileInputModel;
			var command = context.DestinationValue as UpdateUserProfile;

			if (model == null || command == null)
			{
				throw new CannotCreateInputModelException(InputModelType.Name);
			}

			command.Created = DateTime.Now;
			command.CreatedBy = Guid.Empty;
			command.Identifier = Guid.NewGuid();
			command.AvatarUrl = model.AvatarUrl;
			command.Email = model.Email;
			command.UserIdentifier = model.UserIdentifier;

			return command;
		}
	}
}