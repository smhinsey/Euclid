using System;
using System.Data.SqlTypes;
using Euclid.Common.Extensions;
using Euclid.Common.Storage.Model;
using Euclid.Framework.Cqrs;
using ForumAgent.Commands;
using ForumAgent.ReadModels;

namespace ForumAgent.Processors
{
	public class CreateTagProcessor : DefaultCommandProcessor<CreateTag>
	{
		private readonly ISimpleRepository<Tag> _tagRepository;

		public CreateTagProcessor(ISimpleRepository<Tag> tagRepository)
		{
			_tagRepository = tagRepository;
		}

		public override void Process(CreateTag message)
		{
			_tagRepository.Save(new Tag
			                         	{
			                         		Active = message.Active,
			                         		Created = DateTime.Now,
			                         		CreatedBy = message.CreatedBy,
			                         		ForumIdentifier = message.ForumIdentifier,
			                         		Name = message.Name.Slugify(),
			                         		Modified = (DateTime) SqlDateTime.MinValue,
			                         		Identifier = Guid.NewGuid()
			                         	});
		}
	}
}