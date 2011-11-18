using System;
using Euclid.Common.Storage.Model;
using Euclid.Framework.Cqrs;
using ForumAgent.Commands;
using ForumAgent.ReadModels;

namespace ForumAgent.Processors
{
	public class SetForumThemeProcessor : DefaultCommandProcessor<SetForumTheme>
	{
		private readonly ISimpleRepository<Forum> _forumRepository;

		public SetForumThemeProcessor(ISimpleRepository<Forum> forumRepository)
		{
			_forumRepository = forumRepository;
		}

		public override void Process(SetForumTheme message)
		{
			var forum = _forumRepository.FindById(message.ForumIdentifier);

			if (forum == null)
			{
				throw new ForumNotFoundException(string.Format("Unable to set theme for forum with id {0}", message.ForumIdentifier));
			}

			forum.Theme = message.ThemeName;
			forum.Modified = DateTime.Now;

			_forumRepository.Update(forum);
		}
	}
}