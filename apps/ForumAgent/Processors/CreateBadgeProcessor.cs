using System;
using System.Data.SqlTypes;
using Euclid.Common.Storage.Model;
using Euclid.Framework.Cqrs;
using ForumAgent.Commands;
using ForumAgent.ReadModels;

namespace ForumAgent.Processors
{
	public class CreateBadgeProcessor : DefaultCommandProcessor<CreateBadge>
	{
		private readonly ISimpleRepository<ForumBadge> _repository;

		public CreateBadgeProcessor(ISimpleRepository<ForumBadge> repository)
		{
			_repository = repository;
		}

		public override void Process(CreateBadge message)
		{
			_repository.Save(new ForumBadge
			                 	{
			                 		Active = false,
			                 		Name = message.Name,
			                 		Description = message.Description,
			                 		Created = DateTime.Now,
			                 		Modified = (DateTime) SqlDateTime.MinValue,
			                 		ForumIdentifier = message.ForumIdentifier,
			                 		Field = message.Field,
			                 		Operator = message.Operator,
			                 		Value = message.Value,
			                 		ImageUrl = message.ImageUrl
			                 	});
		}
	}
}