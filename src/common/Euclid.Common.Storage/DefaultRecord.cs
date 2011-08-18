using System;

namespace Euclid.Common.Storage
{
	public class DefaultRecord : IRecord
	{
		public virtual DateTime Created { get; set; }
		public virtual Guid CreatedBy { get; set; }
		public virtual Guid Identifier { get; set; }
		public virtual DateTime Modified { get; set; }
	}
}