using System;
using System.Collections.Generic;
using Euclid.Common.Storage.Model;
using NHibernate;

namespace Euclid.Common.Storage.NHibernate
{
	public class NhSimpleRepository : ISimpleRepository
	{
		private readonly ISessionFactory _sessionFactory;

		public NhSimpleRepository(ISessionFactory sessionFactory)
		{
			_sessionFactory = sessionFactory;
		}

		public void Delete(IModel model)
		{
			throw new NotImplementedException();
		}

		public void Delete(Guid id)
		{
			throw new NotImplementedException();
		}

		public IList<IModel> FindByCreationDate(DateTime specificDate)
		{
			throw new NotImplementedException();
		}

		public IList<IModel> FindByCreationDate(DateTime from, DateTime to)
		{
			throw new NotImplementedException();
		}

		public IModel FindById(Guid id)
		{
			throw new NotImplementedException();
		}

		public IList<IModel> FindByModificationDate(DateTime specificDate)
		{
			throw new NotImplementedException();
		}

		public IList<IModel> FindByModificationDate(DateTime from, DateTime to)
		{
			throw new NotImplementedException();
		}

		public void Save(IModel model)
		{
			throw new NotImplementedException();
		}

		public void Update(IModel model)
		{
			throw new NotImplementedException();
		}
	}
}