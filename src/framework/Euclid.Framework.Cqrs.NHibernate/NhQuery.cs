using System;
using System.Collections.Generic;
using Euclid.Common.Storage.NHibernate;
using Euclid.Framework.Models;
using NHibernate;

namespace Euclid.Framework.Cqrs.NHibernate
{
	/// <summary>
	/// 	NhQuery wraps an NhSimpleRepository in order to provide read-only access
	/// 	to a database managed by NHibernate.
	/// </summary>
	/// <typeparam name = "TReadModel"></typeparam>
	public class NhQuery<TReadModel> : IQuery<TReadModel>
		where TReadModel : class, IReadModel
	{
		internal readonly NhSimpleRepository<TReadModel> Repository;

		public NhQuery(ISession session)
		{
			this.Repository = new NhSimpleRepository<TReadModel>(session);
		}

		public IList<TReadModel> FindByCreationDate(DateTime specificDate)
		{
			return this.Repository.FindByCreationDate(specificDate);
		}

		public IList<TReadModel> FindByCreationDate(DateTime begin, DateTime end)
		{
			return this.Repository.FindByCreationDate(begin, end);
		}

		public TReadModel FindById(Guid id)
		{
			return this.Repository.FindById(id);
		}

		public IList<TReadModel> FindByModificationDate(DateTime specificDate)
		{
			return this.Repository.FindByModificationDate(specificDate);
		}

		public IList<TReadModel> FindByModificationDate(DateTime begin, DateTime end)
		{
			return this.Repository.FindByModificationDate(begin, end);
		}

		protected ISession GetCurrentSession()
		{
			return this.Repository.GetCurrentSession();
		}
	}
}