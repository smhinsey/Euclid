using FluentNHibernate.Mapping;

namespace ForumAgent.Domain.Entities.Maps
{
	public class OrganizationUserMap : ClassMap<OrganizationUserEntity>
	{
		public OrganizationUserMap()
		{
			Id(x => x.Identifier);
			References(x => x.OrganizationEntity, "DomainOrganizationIdentifier");
			Map(x => x.Email);
			Map(x => x.FirstName);
			Map(x => x.LastName);
			Map(x => x.PasswordHash);
			Map(x => x.PasswordSalt);
			Map(x => x.Created);
			Map(x => x.Modified);
			Map(x => x.Username);
			Map(x => x.LastLogin);
			Map(x => x.Active);
		}
	}
}