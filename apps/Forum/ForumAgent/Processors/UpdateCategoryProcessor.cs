using System;
using Euclid.Common.Storage.Model;
using Euclid.Framework.Cqrs;
using ForumAgent.Commands;
using ForumAgent.ReadModels;

namespace ForumAgent.Processors
{
	public class UpdateCategoryProcessor : DefaultCommandProcessor<UpdateCategory>
	{
		private readonly ISimpleRepository<Category> _categoryRepository;

		public UpdateCategoryProcessor(ISimpleRepository<Category> categoryRepository)
		{
			_categoryRepository = categoryRepository;
		}

		public override void Process(UpdateCategory message)
		{
			var category = _categoryRepository.FindById(message.CategoryIdentifier);

			if (category == null)
			{
				throw new CategoryNotFoundException(string.Format("Could not update category with id {0}", message.CategoryIdentifier));
			}

			category.Name = message.Name;
			category.Slug = message.Slug;
			category.Active = message.Active;
			category.Modified = DateTime.Now;

			_categoryRepository.Update(category);
		}
	}
}