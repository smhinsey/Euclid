using System;

namespace ForumAgent
{
	public class OrganizationNotFoundException : Exception
	{
		public OrganizationNotFoundException(Guid organizationIdentifier)
			: base(organizationIdentifier.ToString())
		{
		}
	}
}