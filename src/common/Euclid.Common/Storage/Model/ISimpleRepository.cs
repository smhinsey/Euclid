using System;
using System.Collections.Generic;

namespace Euclid.Common.Storage.Model
{
	public interface ISimpleRepository<TModel> : IModelRepository<TModel>
		where TModel : class, IModel
	{
		void Delete(TModel model);

		void Delete(Guid id);

		IList<TModel> FindByCreationDate(DateTime specificDate);

		IList<TModel> FindByCreationDate(DateTime begin, DateTime end);

		TModel FindById(Guid id);

		IList<TModel> FindByModificationDate(DateTime specificDate);

		IList<TModel> FindByModificationDate(DateTime begin, DateTime end);

		TModel Save(TModel model);

		TModel Update(TModel model);
	}
}