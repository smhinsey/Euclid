using System;
using System.Collections.Generic;

namespace Euclid.Common.Storage.Model
{
	public interface ISimpleRepository<TModel> : IModelRepository<TModel>
		where TModel : IModel
	{
		void Delete(TModel model);
		void Delete(Guid id);
		IList<TModel> FindByCreationDate(DateTime specificDate);
		IList<TModel> FindByCreationDate(TimeSpan range);
		TModel FindById(Guid id);
		IList<TModel> FindByModificationDate(DateTime specificDate);
		IList<TModel> FindByModificationDate(TimeSpan range);
		void Save(TModel model);
		void Update(TModel model);
	}
}