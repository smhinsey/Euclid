using System;
using Euclid.Common.Transport;

namespace Euclid.Common.Registry
{
	public interface IRegistry<out TRecord, in TMessage> 
		where TRecord : IRecord<TMessage>, new()
		where TMessage : IMessage
	{
		TRecord CreateRecord(TMessage message);
		TRecord Get(Guid id);

        TRecord MarkAsComplete(Guid id);
	    TRecord MarkAsFailed(Guid id, string message = null, string callStack = null);
	}
}