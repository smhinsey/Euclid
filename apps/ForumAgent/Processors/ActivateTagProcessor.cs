using System;
using Euclid.Common.Storage.Model;
using Euclid.Framework.Cqrs;
using ForumAgent.Commands;
using ForumAgent.ReadModels;

namespace ForumAgent.Processors
{
	public class ActivateTagProcessor : DefaultCommandProcessor<ActivateTag>
	{
		private readonly ISimpleRepository<Tag> _tagRepository;

		public ActivateTagProcessor(ISimpleRepository<Tag> tagRepository)
		{
			_tagRepository = tagRepository;
		}

		public override void Process(ActivateTag message)
		{
			var tag = _tagRepository.FindById(message.TagIdentifier);

			tag.Modified = DateTime.Now;
			tag.Active = message.Active;

			_tagRepository.Update(tag);
		}
	}
}