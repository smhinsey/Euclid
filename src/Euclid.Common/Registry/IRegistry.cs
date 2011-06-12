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

    public interface IRepositoryContext
    {
        
    }

    public interface IBasicRecordRepository<TRecord, in TMessage>
        where TRecord : IRecord<TMessage>
        where TMessage : IMessage
    {
        TRecord Create(TMessage message);
        TRecord Retrieve(Guid id);
        TRecord Update(TRecord record);
        TRecord Delete(Guid id);
    }
}

//[Test]
//public void SaveRecord()
//{
//    var registry = new InMemoryRegistry<FakeRecord, FakeMessage>();

//    var message = new FakeMessage();

//    var record = registry.CreateRecord(message);

//    var retrievedRecord = registry.Get(record.Identifier);

//    Assert.NotNull(retrievedRecord);
//    Assert.AreEqual(retrievedRecord.Identifier, record.Identifier);

//    var r3 = registry.MarkAsError(retrievedRecord.Identifier);

//    Assert.IsTrue(r3.Error);

//    var r4 = registry.MarkAsComplete(r3.Identifier);

//    Assert.IsTrue(r4.Completed);
//}