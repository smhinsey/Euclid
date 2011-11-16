using System;

namespace ForumAgent
{
	public class OrganizationNotFoundException : Exception
	{
		public OrganizationNotFoundException(string message)
			: base(message)
		{

		}

		public OrganizationNotFoundException(Guid organizationIdentifier)
			: base(organizationIdentifier.ToString())
		{
		}
	}
}