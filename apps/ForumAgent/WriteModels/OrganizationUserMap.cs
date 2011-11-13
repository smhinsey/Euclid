using FluentNHibernate.Mapping;

namespace ForumAgent.WriteModels
{
	public class OrganizationUserMap : ClassMap<DomainOrganizationUser>
	{
		public OrganizationUserMap()
		{
			Id(x => x.Identifier);
			References(x => x.Organization, "DomainOrganizationIdentifier");
			Map(x => x.Email);
			Map(x => x.FirstName);
			Map(x => x.LastName);
			Map(x => x.PasswordHash);
			Map(x => x.PasswordSalt);
			Map(x => x.Created);
			Map(x => x.Modified);
			Map(x => x.Username);
		}
	}
}