using System;
using Euclid.Framework.Models;

namespace StorefrontAgent.ReadModels
{
	public class Transaction : DefaultReadModel
	{
		public virtual Guid CompanyIdentifier { get; set; }

		public virtual TransactionState State { get; set; }

		public virtual DateTime InitiationDate { get; set; }

		public virtual TransactionType Type { get; set; }

		public virtual decimal Amount { get; set; }
	}
}