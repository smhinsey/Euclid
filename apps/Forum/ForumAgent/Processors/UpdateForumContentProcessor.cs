using System;
using Euclid.Common.Storage.Model;
using Euclid.Framework.Cqrs;
using ForumAgent.Commands;
using ForumAgent.ReadModels;

namespace ForumAgent.Processors
{
	public class UpdateForumContentProcessor : DefaultCommandProcessor<UpdateForumContent>
	{
		private readonly ISimpleRepository<ForumContent> _contentRepository;

		public UpdateForumContentProcessor(ISimpleRepository<ForumContent> contentRepository)
		{
			_contentRepository = contentRepository;
		}

		public override void Process(UpdateForumContent message)
		{
			var content = _contentRepository.FindById(message.ContentIdentifier);

			if (content == null)
			{
				throw new ContentNotFoundException(string.Format("Can not update the content with identifier {0}",
				                                                 message.ContentIdentifier));
			}

			content.ContentLocation = message.Location;
			content.ContentType = message.Type;
			content.Modified = DateTime.Now;
			content.Active = message.Active;
			content.Value = message.Value;

			_contentRepository.Update(content);
		}
	}
}