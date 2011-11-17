using System;
using System.Data.SqlTypes;
using Euclid.Common.Storage.Model;
using Euclid.Framework.Cqrs;
using ForumAgent.Commands;
using ForumAgent.ReadModels;

namespace ForumAgent.Processors
{
	public class CreateForumContentProcessor : DefaultCommandProcessor<CreateForumContent>
	{
		private readonly ISimpleRepository<ForumContent> _contentRepository;

		public CreateForumContentProcessor(ISimpleRepository<ForumContent> contentRepository)
		{
			_contentRepository = contentRepository;
		}

		public override void Process(CreateForumContent message)
		{
			_contentRepository.Save(new ForumContent
			                        	{
			                        		Active = message.Active,
			                        		ContentLocation = message.Location,
			                        		ContentType = message.Type,
			                        		CreatedBy = message.CreatedBy,
			                        		ForumIdentifier = message.ForumIdentifier,
											Value = message.Value,
			                        		Created = DateTime.Now,
			                        		Modified = (DateTime) SqlDateTime.MinValue,
			                        		Identifier = Guid.NewGuid()
			                        	});
		}
	}
}