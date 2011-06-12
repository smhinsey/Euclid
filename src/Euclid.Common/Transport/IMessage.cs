using System;

namespace Euclid.Common.Transport
{
	public interface IMessage
	{
		DateTime Created { get; set; }
		Guid CreatedBy { get; set; }
		Guid Identifier { get; set; }
	}
}