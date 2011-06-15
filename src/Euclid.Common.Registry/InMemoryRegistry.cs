using System;
using Euclid.Common.Storage;
using Euclid.Common.Transport;

namespace Euclid.Common.Registry
{
	public class InMemoryRegistry<TRecord> : IRegistry<TRecord>
        where TRecord : class, IRecord, new()
	{
		private readonly IBasicRecordRepository<TRecord> _repository;

	    public InMemoryRegistry(IBasicRecordRepository<TRecord> repository)
	    {
	        _repository = repository;
	    }

	    public TRecord CreateRecord(IMessage message)
	    {
	        return _repository.Create(message);
	    }

	    public TRecord Get(Guid id)
	    {
	        return _repository.Retrieve(id);
	    }

	    public TRecord MarkAsComplete(Guid id)
	    {
	        return UpdateRecord(id, r => r.Completed = true);
	    }

	    public TRecord MarkAsFailed(Guid id, string message, string callStack)
	    {
	        return UpdateRecord(id, r =>
	                                    {
	                                        r.Completed = true;
	                                        r.Error = true;
	                                        r.ErrorMessage = message;
	                                        r.CallStack = callStack;
	                                    });
	    }

        private TRecord UpdateRecord(Guid id, Action<TRecord> actOnRecord)
        {
            var record = Get(id);

            actOnRecord(record);

            return _repository.Update(record);
        }
	}
}