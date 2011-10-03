using System;
using AdminComposite.Models;
using AutoMapper;
using Euclid.Framework.Cqrs;
using ForumAgent.Commands;

namespace AdminComposite.Converters
{
	public class CreateForumInputModelConverter : IInputToCommandConverter
	{
		public ICommand Convert(ResolutionContext context)
		{
			var model = context.SourceValue as CreateForumInputModel;
			var command = context.DestinationValue as CreateForum;

			if (model == null || command == null)
			{
				throw new CannotCreateInputModelException(InputModelType.Name);
			}

			command.Created = DateTime.Now;
			command.Identifier = Guid.NewGuid();

			command.Name = model.Name;
			command.UrlHostName = model.UrlHostName;
			command.UrlSlug = model.UrlSlug;

			return command;
		}

		public Type CommandType
		{
			get
			{
				return typeof(CreateForum);
			}
		}

		public Type InputModelType
		{
			get
			{
				return typeof(CreateForumInputModel);
			}
		}
	}
}