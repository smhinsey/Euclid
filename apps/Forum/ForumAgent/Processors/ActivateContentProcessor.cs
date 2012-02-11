using System;
using Euclid.Common.Storage.Model;
using Euclid.Framework.Cqrs;
using ForumAgent.Commands;
using ForumAgent.ReadModels;

namespace ForumAgent.Processors
{
	public class ActivateContentProcessor : DefaultCommandProcessor<ActivateContent>
	{
		private readonly ISimpleRepository<ForumContent> _contentRepository;

		public ActivateContentProcessor(ISimpleRepository<ForumContent> contentRepository)
		{
			_contentRepository = contentRepository;
		}

		public override void Process(ActivateContent message)
		{
			var content = _contentRepository.FindById(message.ContentIdentifier);

			if (content == null)
			{
				throw new ForumContentNotFoundException(
					string.Format("Cannot process activation request for ForumContent with id {0}", message.ContentIdentifier));
			}

			content.Modified = DateTime.Now;
			content.Active = message.Active;

			_contentRepository.Update(content);
		}
	}
}