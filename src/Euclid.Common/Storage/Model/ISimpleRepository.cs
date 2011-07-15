using System;
using System.Collections.Generic;

namespace Euclid.Common.Storage.Model
{
	public interface ISimpleRepository : IModelRepository
	{
		IList<IModel> FindByCreationDate(DateTime specificDate);
		IList<IModel> FindByCreationDate(DateTime from, DateTime to);
		IModel FindById(Guid id);
		IList<IModel> FindByModificationDate(DateTime specificDate);
		IList<IModel> FindByModificationDate(DateTime from, DateTime to);
		void Save(IModel model);
		void Update(IModel model);
		void Delete(IModel model);
		void Delete(Guid id);
	}
}