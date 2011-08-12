using System;
using System.Collections.Generic;
using Euclid.Common.Storage.Model;
using NHibernate;

namespace Euclid.Common.Storage.NHibernate
{
	public class NhSimpleRepository<TModel> : NhSessionConsumer, ISimpleRepository<TModel>
		where TModel : class, IModel
	{
		public NhSimpleRepository(ISession session) : base(session)
		{
		}

		public void Delete(TModel model)
		{
			var session = GetCurrentSession();

			using (var transaction = session.BeginTransaction())
			{
				try
				{
					session.Delete(model);

					transaction.Commit();
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
			var session = GetCurrentSession();

			using (var transaction = session.BeginTransaction())
			{
				try
				{
					var model = session.Get(typeof (TModel), id);

					session.Delete(model);

					transaction.Commit();
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
			var session = GetCurrentSession();

			var query = session.QueryOver<TModel>()
				.Where(x => x.Created == specificDate);

			return query.List();
		}

		public IList<TModel> FindByCreationDate(DateTime begin, DateTime end)
		{
			var session = GetCurrentSession();

			var query = session.QueryOver<TModel>()
				.WhereRestrictionOn(x => x.Created).IsBetween(begin).And(end);

			return query.List();
		}

		public TModel FindById(Guid id)
		{
			var session = GetCurrentSession();

			var query = session.QueryOver<TModel>()
				.Where(x => x.Identifier == id);

			return query.SingleOrDefault();
		}

		public IList<TModel> FindByModificationDate(DateTime specificDate)
		{
			var session = GetCurrentSession();

			var query = session.QueryOver<TModel>()
				.Where(x => x.Modified == specificDate);

			return query.List();
		}

		public IList<TModel> FindByModificationDate(DateTime begin, DateTime end)
		{
			var session = GetCurrentSession();

			var query = session.QueryOver<TModel>()
				.WhereRestrictionOn(x => x.Modified).IsBetween(begin).And(end);

			return query.List();
		}

		public TModel Save(TModel model)
		{
			var session = GetCurrentSession();

			using (var transaction = session.BeginTransaction())
			{
				try
				{
					session.Save(model);

					transaction.Commit();
				}
				catch (Exception)
				{
					transaction.Rollback();

					throw;
				}
			}

			return model;
		}

		public TModel Update(TModel model)
		{
			var session = GetCurrentSession();

			using (var transaction = session.BeginTransaction())
			{
				try
				{
					session.Update(model);

					transaction.Commit();
				}
				catch (Exception)
				{
					transaction.Rollback();

					throw;
				}
			}

			return model;
		}
	}
}