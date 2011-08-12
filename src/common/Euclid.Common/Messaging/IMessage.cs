using System;

namespace Euclid.Common.Messaging
{
	public interface IMessage
	{
		DateTime Created { get; set; }
		Guid CreatedBy { get; set; }
		Guid Identifier { get; set; }
	}
}