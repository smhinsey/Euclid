using System;
using System.Collections.Generic;
using Euclid.Common.Logging;
using Euclid.Common.Storage.Model;
using NHibernate;

namespace Euclid.Common.Storage.NHibernate
{
	public class NhSimpleRepository<TModel> : NhSessionConsumer, ISimpleRepository<TModel>, ILoggingSource
		where TModel : class, IModel
	{
		public NhSimpleRepository(ISession session)
			: base(session)
		{
		}

		public void Delete(TModel model)
		{
			var session = GetCurrentSession();

			this.WriteDebugMessage(string.Format("Deleting model {0}({1})", model.GetType().Name, model.Identifier));

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

			this.WriteDebugMessage(string.Format("Deleted model {0}({1})", model.GetType().Name, model.Identifier));
		}

		public void Delete(Guid id)
		{
			var session = GetCurrentSession();

			using (var transaction = session.BeginTransaction())
			{
				try
				{
					var model = (TModel)session.Get(typeof(TModel), id);

					this.WriteDebugMessage(string.Format("Deleting model {0}({1})", model.GetType().Name, model.Identifier));

					session.Delete(model);

					transaction.Commit();

					this.WriteDebugMessage(string.Format("Deleted model {0}({1})", model.GetType().Name, model.Identifier));
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

			var query = session.QueryOver<TModel>().Where(x => x.Created == specificDate);

			return query.List();
		}

		public IList<TModel> FindByCreationDate(DateTime begin, DateTime end)
		{
			var session = GetCurrentSession();

			var query = session.QueryOver<TModel>().WhereRestrictionOn(x => x.Created).IsBetween(begin).And(end);

			return query.List();
		}

		public TModel FindById(Guid id)
		{
			var session = GetCurrentSession();

			var query = session.QueryOver<TModel>().Where(x => x.Identifier == id);

			return query.SingleOrDefault();
		}

		public IList<TModel> FindByModificationDate(DateTime specificDate)
		{
			var session = GetCurrentSession();

			var query = session.QueryOver<TModel>().Where(x => x.Modified == specificDate);

			return query.List();
		}

		public IList<TModel> FindByModificationDate(DateTime begin, DateTime end)
		{
			var session = GetCurrentSession();

			var query = session.QueryOver<TModel>().WhereRestrictionOn(x => x.Modified).IsBetween(begin).And(end);

			return query.List();
		}

		public TModel Save(TModel model)
		{
			var session = GetCurrentSession();

			this.WriteDebugMessage(string.Format("Saving model {0}({1})", model.GetType().Name, model.Identifier));

			using (var transaction = session.BeginTransaction())
			{
				try
				{
					session.Save(model);

					transaction.Commit();

					this.WriteDebugMessage(string.Format("Saved model {0}({1})", model.GetType().Name, model.Identifier));
				}
				catch (Exception)
				{
					transaction.Rollback();

					this.WriteDebugMessage(string.Format("Failed to save model {0}({1})", model.GetType().Name, model.Identifier));

					throw;
				}
			}

			return model;
		}

		public TModel Update(TModel model)
		{
			var session = GetCurrentSession();

			this.WriteDebugMessage(string.Format("Updating model {0}({1})", model.GetType().Name, model.Identifier));

			using (var transaction = session.BeginTransaction())
			{
				try
				{
					session.Update(model);

					transaction.Commit();

					this.WriteDebugMessage(string.Format("Updated model {0}({1})", model.GetType().Name, model.Identifier));
				}
				catch (Exception)
				{
					transaction.Rollback();

					this.WriteDebugMessage(string.Format("Failed to update model {0}({1})", model.GetType().Name, model.Identifier));

					throw;
				}
			}

			return model;
		}
	}
}