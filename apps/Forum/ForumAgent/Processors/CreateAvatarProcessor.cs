using System;
using System.Data.SqlTypes;
using Euclid.Common.Storage.Model;
using Euclid.Framework.Cqrs;
using ForumAgent.Commands;
using ForumAgent.ReadModels;

namespace ForumAgent.Processors
{
	public class CreateAvatarProcessor : DefaultCommandProcessor<CreateAvatar>
	{
		private readonly ISimpleRepository<ForumAvatar> _repository;

		public CreateAvatarProcessor(ISimpleRepository<ForumAvatar> repository)
		{
			_repository = repository;
		}

		public override void Process(CreateAvatar message)
		{
			_repository.Save(new ForumAvatar
			                 	{
			                 		Active = false,
			                 		Created = DateTime.Now,
			                 		Modified = (DateTime) SqlDateTime.MinValue,
			                 		Description = message.Description,
			                 		ForumIdentifier = message.ForumIdentifier,
			                 		Name = message.Name,
			                 		Url = message.ImageUrl,
									CreatedBy = message.CreatedBy
			                 	});
		}
	}
}