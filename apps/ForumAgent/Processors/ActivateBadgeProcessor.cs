using System;
using Euclid.Common.Storage.Model;
using Euclid.Framework.Cqrs;
using ForumAgent.Commands;
using ForumAgent.ReadModels;

namespace ForumAgent.Processors
{
	public class ActivateBadgeProcessor : DefaultCommandProcessor<ActivateBadge>
	{
		private readonly ISimpleRepository<ForumBadge> _repository;

		public ActivateBadgeProcessor(ISimpleRepository<ForumBadge> repository)
		{
			_repository = repository;
		}

		public override void Process(ActivateBadge message)
		{
			var badge = _repository.FindById(message.BadgeIdentifier);

			badge.Modified = DateTime.Now;
			badge.Active = message.Active;

			_repository.Update(badge);
		}
	}
}