using System;

namespace Euclid.Framework
{
	public class DefaultReadModel : IReadModel
	{
		public virtual Guid Identifier { get; set; }
	}
}