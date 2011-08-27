using System;

namespace Euclid.Framework.Cqrs
{
	public abstract class DefaultCommand : ICommand
	{
		public DateTime Created { get; set; }

		public Guid CreatedBy { get; set; }

		public Guid Identifier { get; set; }
	}
}