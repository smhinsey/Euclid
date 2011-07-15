using System;
using System.Collections.Generic;
using Euclid.Common.Storage.Model;
using NHibernate;

namespace Euclid.Common.Storage.NHibernate
{
	public class NhSimpleRepository<TModel> : ISimpleRepository<TModel>
		where TModel : class, IModel
	{
		private readonly ISessionFactory _sessionFactory;

		public NhSimpleRepository(ISessionFactory sessionFactory)
		{
			_sessionFactory = sessionFactory;
		}

		public void Delete(TModel model)
		{
			using (var session = _sessionFactory.OpenSession())
			using (var transaction = session.BeginTransaction())
			{
				try
				{
					session.Delete(model);
				}
				catch (Exception)
				{
					transaction.Rollback();

					throw;
				}
			}
	}

		public void Delete(Guid id)
		{
			using (var session = _sessionFactory.OpenSession())
			using (var transaction = session.BeginTransaction())
			{
				try
				{
					var model = session.Get(typeof(TModel), id);

					session.Delete(model);
				}
				catch (Exception)
				{
					transaction.Rollback();

					throw;
				}
			}
		}

		public IList<TModel> FindByCreationDate(DateTime specificDate)
		{
			using (var session = _sessionFactory.OpenSession())
			{
				var query = session.QueryOver<TModel>()
					.Where(x => x.Created == specificDate);

				return query.List();
			}
		}

		public IList<TModel> FindByCreationDate(DateTime begin, DateTime end)
		{
			using (var session = _sessionFactory.OpenSession())
			{
				var query = session.QueryOver<TModel>()
					.WhereRestrictionOn(x => x.Created).IsBetween(begin).And(end);

				return query.List();
			}
		}

		public TModel FindById(Guid id)
		{
			using (var session = _sessionFactory.OpenSession())
			{
				var query = session.QueryOver<TModel>()
					.Where(x => x.Identifier == id);

				return query.SingleOrDefault();
			}
		}

		public IList<TModel> FindByModificationDate(DateTime specificDate)
		{
			using (var session = _sessionFactory.OpenSession())
			{
				var query = session.QueryOver<TModel>()
					.Where(x => x.Modified == specificDate);

				return query.List();
			}
		}

		public IList<TModel> FindByModificationDate(DateTime begin, DateTime end)
		{
			using (var session = _sessionFactory.OpenSession())
			{
				var query = session.QueryOver<TModel>()
					.WhereRestrictionOn(x => x.Modified).IsBetween(begin).And(end);

				return query.List();
			}
		}

		public void Save(TModel model)
		{
			using (var session = _sessionFactory.OpenSession())
			using (var transaction = session.BeginTransaction())
			{
				try
				{
					session.Save(model);
				}
				catch (Exception)
				{
					transaction.Rollback();

					throw;
				}
			}
		}

		public void Update(TModel model)
		{
			using (var session = _sessionFactory.OpenSession())
			using (var transaction = session.BeginTransaction())
			{
				try
				{
					session.Update(model);
				}
				catch (Exception)
				{
					transaction.Rollback();

					throw;
				}
			}
		}
	}
}