using Euclid.Common.Storage.NHibernate;
using ForumAgent.Domain.Entities;
using NHibernate;

namespace ForumAgent.Queries
{
	public class OrganizationQueries
	{
		private readonly NhSimpleRepository<OrganizationEntity> _repository;

		private readonly ISession _session;

		public OrganizationQueries(ISession session)
		{
			_session = session;
			_repository = new NhSimpleRepository<OrganizationEntity>(session);
		}
	}
}