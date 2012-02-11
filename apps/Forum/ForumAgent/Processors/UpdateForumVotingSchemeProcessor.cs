using System;
using Euclid.Common.Storage.Model;
using Euclid.Framework.Cqrs;
using ForumAgent.Commands;
using ForumAgent.ReadModels;

namespace ForumAgent.Processors
{
	public class UpdateForumVotingSchemeProcessor : DefaultCommandProcessor<UpdateForumVotingScheme>
	{
		private readonly ISimpleRepository<Forum> _forumRepository;

		public UpdateForumVotingSchemeProcessor(ISimpleRepository<Forum> forumRepository)
		{
			_forumRepository = forumRepository;
		}

		public override void Process(UpdateForumVotingScheme message)
		{
			var forum = _forumRepository.FindById(message.ForumIdentifier);

			if (forum == null)
			{
				throw new ForumNotFoundException(string.Format("Cannot update voting scheme for the forum with id '{0}'", message.ForumIdentifier));
			}

			forum.UpDownVoting = message.UpDownVoting;
			forum.NoVoting = message.NoVoting;
			forum.Modified = DateTime.Now;

			_forumRepository.Save(forum);
		}
	}
}