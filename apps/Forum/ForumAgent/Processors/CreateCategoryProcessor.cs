using System;
using System.Data.SqlTypes;
using Euclid.Common.Storage.Model;
using Euclid.Framework.Cqrs;
using ForumAgent.Commands;
using ForumAgent.ReadModels;

namespace ForumAgent.Processors
{
	public class CreateCategoryProcessor : DefaultCommandProcessor<CreateCategory>
	{
		private readonly ISimpleRepository<Category> _categoryRepository;

		public CreateCategoryProcessor(ISimpleRepository<Category> categoryRepository)
		{
			_categoryRepository = categoryRepository;
		}

		public override void Process(CreateCategory message)
		{
			_categoryRepository.Save(new Category
			                         	{
			                         		Active = message.Active,
			                         		Created = DateTime.Now,
			                         		CreatedBy = message.CreatedBy,
			                         		ForumIdentifier = message.ForumIdentifier,
			                         		Name = message.Name,
			                         		Slug = message.Slug,
			                         		Modified = (DateTime) SqlDateTime.MinValue,
			                         		Identifier = Guid.NewGuid()
			                         	});
		}
	}
}