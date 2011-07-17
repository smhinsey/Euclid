using System;

namespace Euclid.Framework
{
	public class DefaultReadModel : IReadModel
	{
		public DateTime Created { get; set; }
		public virtual Guid Identifier { get; set; }
		public DateTime Modified { get; set; }
	}
}