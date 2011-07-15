using System;
using System.Collections.Generic;
using Euclid.Common.Storage.Model;
using NHibernate;

namespace Euclid.Common.Storage.NHibernate
{
	public class NhSimpleRepository<TModel> : ISimpleRepository<TModel>
		where TModel : IModel
	{
		private readonly ISessionFactory _sessionFactory;

		public NhSimpleRepository(ISessionFactory sessionFactory)
		{
			_sessionFactory = sessionFactory;
		}

		public void Delete(TModel model)
		{
			throw new NotImplementedException();
		}

		public void Delete(Guid id)
		{
			throw new NotImplementedException();
		}

		public IList<TModel> FindByCreationDate(DateTime specificDate)
		{
			throw new NotImplementedException();
		}

		public IList<TModel> FindByCreationDate(TimeSpan range)
		{
			throw new NotImplementedException();
		}

		public TModel FindById(Guid id)
		{
			throw new NotImplementedException();
		}

		public IList<TModel> FindByModificationDate(DateTime specificDate)
		{
			throw new NotImplementedException();
		}

		public IList<TModel> FindByModificationDate(TimeSpan range)
		{
			throw new NotImplementedException();
		}

		public void Save(TModel model)
		{
			throw new NotImplementedException();
		}

		public void Update(TModel model)
		{
			throw new NotImplementedException();
		}
	}
}