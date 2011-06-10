using System;

namespace Euclid.Common.Transport
{
	public interface IMessage
	{
		Guid Identifier { get; set; }
		Guid CreatedBy { get; set; }
		DateTime Created { get; set; }

	}
}