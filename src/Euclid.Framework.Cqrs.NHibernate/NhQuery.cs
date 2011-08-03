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
		private readonly NhSimpleRepository<TReadModel> _repository;

		public NhQuery(ISession session)
		{
			_repository = new NhSimpleRepository<TReadModel>(session);
		}

		public IList<TReadModel> FindByCreationDate(DateTime specificDate)
		{
			return _repository.FindByCreationDate(specificDate);
		}

		public IList<TReadModel> FindByCreationDate(DateTime begin, DateTime end)
		{
			return _repository.FindByCreationDate(begin, end);
		}

		public TReadModel FindById(Guid id)
		{
			return _repository.FindById(id);
		}

		public IList<TReadModel> FindByModificationDate(DateTime specificDate)
		{
			return _repository.FindByModificationDate(specificDate);
		}

		public IList<TReadModel> FindByModificationDate(DateTime begin, DateTime end)
		{
			return _repository.FindByModificationDate(begin, end);
		}

		protected ISession GetCurrentSession()
		{
			return _repository.GetCurrentSession();
		}
	}
}