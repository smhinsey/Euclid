using FluentNHibernate.Mapping;

namespace ForumAgent.WriteModels
{
	public class OrganizationMap : ClassMap<DomainOrganization>
	{
		public OrganizationMap()
		{
			Id(x => x.Identifier);
			HasMany(x => x.Users);
			Map(x=>x.OrganizationName);
			Map(x=>x.OrganizationUrl);
			Map(x=>x.PhoneNumber);
			Map(x=>x.Address);
			Map(x=>x.Address2);
			Map(x=>x.City);
			Map(x=>x.State);
			Map(x=>x.Zip);
			Map(x=>x.Country);
			Map(x => x.Created);
			Map(x => x.Modified);
		}
	}
}