using Euclid.Common.Storage.NHibernate;
using Euclid.Framework.Models;

namespace Euclid.Framework.Cqrs.NHibernate
{
	/// <summary>
	/// Extracts an internal repository from a query
	/// </summary>
	public class RepositoryExtractor<TReadModel>
		where TReadModel : class, IReadModel
	{
		public NhSimpleRepository<TReadModel> ExtractRepository(NhQuery<TReadModel> query)
		{
			return query.Repository;
		}
	}
}