using System;
using Euclid.Common.Storage.Model;
using Euclid.Framework.Cqrs;
using ForumAgent.Commands;
using ForumAgent.ReadModels;

namespace ForumAgent.Processors
{
	public class ActivateCategoryProcessor : DefaultCommandProcessor<ActivateCategory>
	{
		private readonly ISimpleRepository<Category> _categoryRepository;

		public ActivateCategoryProcessor(ISimpleRepository<Category> categoryRepository)
		{
			_categoryRepository = categoryRepository;
		}

		public override void Process(ActivateCategory message)
		{
			var category = _categoryRepository.FindById(message.CategoryIdentifier);

			if (category == null)
			{
				throw new CategoryNotFoundException(string.Format("Unable to activate the category with id '{0}'", message.CategoryIdentifier));
			}

			category.Modified = DateTime.Now;
			category.Active = message.Active;

			_categoryRepository.Update(category);
		}
	}
}