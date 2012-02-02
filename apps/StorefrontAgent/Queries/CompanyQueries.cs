using System;
using System.Collections.Generic;
using Euclid.Framework.Cqrs.NHibernate;
using NHibernate;
using StorefrontAgent.ReadModels;

namespace StorefrontAgent.Queries
{
	public class CompanyQueries : NhQuery<Company>
	{
		public CompanyQueries(ISession session)
			: base(session)
		{
		}

		public IList<CompanyEmployee> FindEmployeesByCompany()
		{
			var results = new List<CompanyEmployee>
				{
					new CompanyEmployee
						{
							FirstName = "John",
							LastName = "Doe",
							HireDate = DateTime.Now.AddMonths(-5),
							Location = "Atlanta",
							Type = EmployeeType.Admin
						},
					new CompanyEmployee
						{
							FirstName = "Jane",
							LastName = "Doe",
							HireDate = DateTime.Now.AddMonths(-4),
							Location = "Boston",
							Type = EmployeeType.Admin
						},
					new CompanyEmployee
						{
							FirstName = "Jack",
							LastName = "Doe",
							HireDate = DateTime.Now.AddMonths(-19),
							Location = "Charlotte",
							Type = EmployeeType.StoreManagement
						},
					new CompanyEmployee
						{
							FirstName = "Jill",
							LastName = "Doe",
							HireDate = DateTime.Now.AddMonths(-9),
							Location = "D.C.",
							Type = EmployeeType.StoreManagement
						},
				};

			return results;
		}
	}
}