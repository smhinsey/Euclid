using System;
using Euclid.Common.Storage.Model;
using Euclid.Framework.Cqrs;
using ForumAgent.Commands;
using ForumAgent.ReadModels;

namespace ForumAgent.Processors
{
	public class ActivateForumUserProcessor : DefaultCommandProcessor<ActivateForumUser>
	{
		private readonly ISimpleRepository<ForumUser> _repository;

		public ActivateForumUserProcessor(ISimpleRepository<ForumUser> repository)
		{
			_repository = repository;
		}

		public override void Process(ActivateForumUser message)
		{
			var user = _repository.FindById(message.UserIdentifier);

			user.Active = message.Active;
			user.Modified = DateTime.Now;

			_repository.Update(user);
		}
	}
}