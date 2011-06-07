using System;
using System.Collections.Generic;

namespace Euclid.Common.Transport
{
	public interface IMessageTransport : ITypeTransport<IEnvelope>
	{
	}
}