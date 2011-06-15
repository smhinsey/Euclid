using System;
using Euclid.Common.Transport;

namespace Euclid.Common.Registry
{
	public interface IRegistry<out TRecord>
		where TRecord : class, IRecord, new()
	{
		TRecord CreateRecord(IMessage message);
		TRecord Get(Guid id);

		TRecord MarkAsComplete(Guid id);
		TRecord MarkAsFailed(Guid id, string message = null, string callStack = null);
	}
}