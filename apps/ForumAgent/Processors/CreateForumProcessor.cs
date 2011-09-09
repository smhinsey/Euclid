﻿using Euclid.Common.Storage.Model;
using Euclid.Framework.Cqrs;
using ForumAgent.Commands;
using ForumAgent.ReadModels;

namespace ForumAgent.Processors
{
	public class CreateForumProcessor : DefaultCommandProcessor<CreateForum>
	{
		private readonly ISimpleRepository<Forum> _forumRepository;

		public CreateForumProcessor(ISimpleRepository<Forum> forumRepository)
		{
			_forumRepository = forumRepository;
		}

		public override void Process(CreateForum message)
		{
			var forum = new Forum
				{
					Name = message.Name,
					UrlHostName = message.UrlHostName,
					UrlSlug = message.UrlSlug,
					Created = message.Created,
					Modified = message.Created
				};

			_forumRepository.Save(forum);
		}
	}
}