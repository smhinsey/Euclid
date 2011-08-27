using System;

namespace Euclid.Framework.Cqrs
{
	public class CommandPublicationRecord : ICommandPublicationRecord
	{
		public virtual string CallStack { get; set; }

		public virtual bool Completed { get; set; }

		public virtual DateTime Created { get; set; }

		public virtual Guid CreatedBy { get; set; }

		public virtual bool Dispatched { get; set; }

		public virtual bool Error { get; set; }

		public virtual string ErrorMessage { get; set; }

		public virtual Guid Identifier { get; set; }

		public virtual Uri MessageLocation { get; set; }

		public virtual Type MessageType { get; set; }
	}
}