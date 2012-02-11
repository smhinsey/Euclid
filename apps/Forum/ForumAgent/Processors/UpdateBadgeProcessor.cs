using System;
using Euclid.Common.Storage.Model;
using Euclid.Framework.Cqrs;
using ForumAgent.Commands;
using ForumAgent.ReadModels;

namespace ForumAgent.Processors
{
	public class UpdateBadgeProcessor : DefaultCommandProcessor<UpdateBadge>
	{
		private readonly ISimpleRepository<ForumBadge> _repository;

		public UpdateBadgeProcessor(ISimpleRepository<ForumBadge> repository)
		{
			_repository = repository;
		}

		public override void Process(UpdateBadge message)
		{
			var badge = _repository.FindById(message.BadgeIdentifier);

			badge.Modified = DateTime.Now;
			badge.Field = message.Field;
			badge.ImageUrl = message.ImageUrl;
			badge.Name = message.Name;
			badge.Operator = message.Operator;
			badge.Value = message.Value;

			_repository.Update(badge);
		}
	}
}