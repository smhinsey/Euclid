using System;
using AutoMapper;
using Euclid.Composites.Conversion;
using Euclid.Framework.Cqrs;
using ForumAgent.Commands;
using ForumComposite.Models;

namespace ForumComposite.Converters
{
	public class RegisterUserInputModelConverter : IInputToCommandConverter
	{
		public Type CommandType
		{
			get { return typeof(RegisterUser); }
		}

		public Type InputModelType
		{
			get { return typeof(RegisterUserInputModel); }
		}

		public ICommand Convert(ResolutionContext context)
		{
			var model = context.SourceValue as RegisterUserInputModel;
			var command = context.DestinationValue as RegisterUser;

			if (model == null || command == null)
			{
				throw new CannotCreateInputModelException(InputModelType.Name);
			}

			command.Created = DateTime.Now;
			command.CreatedBy = Guid.Empty;
			command.Identifier = Guid.NewGuid();
			command.Username = model.Username;

			// SELF hash the password
			command.PasswordHash = model.Password;
			command.PasswordSalt = model.Password;

			return command;
		} 
	}
}