using FluentNHibernate.Mapping;

namespace ForumAgent.Domain.Entities.Maps
{
	public class OrganizationMap : ClassMap<OrganizationEntity>
	{
		public OrganizationMap()
		{
			Id(x => x.Identifier);
			HasMany(x => x.Users);
			Map(x => x.OrganizationName);
			Map(x => x.OrganizationSlug);
			Map(x => x.OrganizationUrl);
			Map(x => x.PhoneNumber);
			Map(x => x.Address);
			Map(x => x.Address2);
			Map(x => x.City);
			Map(x => x.State);
			Map(x => x.Zip);
			Map(x => x.Country);
			Map(x => x.Created);
			Map(x => x.Modified);
			Map(x => x.CreatedBy);
		}
	}
}