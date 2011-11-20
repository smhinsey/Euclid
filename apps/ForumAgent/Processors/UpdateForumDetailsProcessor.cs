using System;
using Euclid.Common.Storage.Model;
using Euclid.Framework.Cqrs;
using ForumAgent.Commands;
using ForumAgent.ReadModels;

namespace ForumAgent.Processors
{
	public class UpdateForumDetailsProcessor : DefaultCommandProcessor<UpdateForum>
	{
		private readonly ISimpleRepository<Forum> _forumRepository;

		public UpdateForumDetailsProcessor(ISimpleRepository<Forum> forumRepository)
		{
			_forumRepository = forumRepository;
		}

		public override void Process(UpdateForum message)
		{
			var forum = _forumRepository.FindById(message.ForumIdentifier);

			if (forum == null)
			{
				throw new ForumNotFoundException(string.Format("Can not update forum, the forum with id '{0}' cannot be found",
				                                               message.Identifier));
			}

			forum.Name = message.Name;
			forum.UrlHostName = message.UrlHostName;
			forum.UrlSlug = message.UrlSlug;
			forum.Description = message.Description;
			forum.Private = message.Private;
			forum.Moderated = message.Moderated;
			forum.UrlHostName = message.UrlHostName;
			forum.Modified = DateTime.Now;
			
			_forumRepository.Save(forum);
		}
	}
}