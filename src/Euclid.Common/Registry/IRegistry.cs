using System;
using Euclid.Common.Transport;

namespace Euclid.Common.Registry
{
	public interface IRegistry<TRecord, in TMessage> 
		where TRecord : IRecord<TMessage> 
		where TMessage : IMessage
	{
		void Add(TRecord record);
		TRecord CreateRecord(TMessage message);
		TRecord Get(Guid id);
	}
}