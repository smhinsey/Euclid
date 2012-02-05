using System;

namespace StorefrontAgent.ReadModels
{
	public class CompanyEmployee
	{
		public virtual string FirstName { get; set; }

		public virtual string LastName { get; set; }

		public virtual EmployeeType Type { get; set; }

		public virtual DateTime HireDate { get; set; }

		public virtual string Location { get; set; }
	}
}