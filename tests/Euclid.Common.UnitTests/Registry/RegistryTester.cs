using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Euclid.Common.Registry;
using Euclid.Common.TestingFakes.Registry;
using Euclid.Common.Transport;
using NUnit.Framework;

namespace Euclid.Common.UnitTests.Registry
{
	public class RegistryTester<TRegistry>
		where TRegistry : IRegistry<FakeRecord>
	{
		private readonly TRegistry _registry;

		public RegistryTester(TRegistry registry)
		{
			_registry = registry;
		}

		public FakeRecord CreateRecord(IMessage message)
		{
			var record = _registry.CreateRecord(message);

			Assert.NotNull(record);

			Assert.AreEqual(message.GetType(), record.MessageType);

			return record;
		}

		public IMessage GetMessage(FakeRecord record)
		{
			var message = _registry.GetMessage(record.MessageLocation, record.MessageType);

			Assert.NotNull(message);

			Assert.AreEqual(record.MessageType, message.GetType());

			return message;
		}

		public FakeRecord GetRecord()
		{
			var record = CreateRecord(new FakeMessage());

			Assert.NotNull(record);

			var retrieved = _registry.GetRecord(record.Identifier);

			Assert.AreEqual(record.Identifier, retrieved.Identifier);

			return retrieved;
		}


		public FakeRecord MarkAsCompleted()
		{
			var record = CreateRecord(new FakeMessage());

			Assert.NotNull(record);

			record = _registry.GetRecord(record.Identifier);

			Assert.NotNull(record);

			Assert.IsFalse(record.Completed);

			record = _registry.MarkAsComplete(record.Identifier);

			Assert.NotNull(record);

			Assert.IsTrue(record.Completed);

            Assert.IsTrue(record.Dispatched);

			return record;
		}

		public IRecord MarkAsFailed()
		{
			const string errorMessage = "test error message";

			const string callStack = "call stack 1";

			var record = CreateRecord(new FakeMessage());

			Assert.NotNull(record);

			record = _registry.GetRecord(record.Identifier);

			Assert.NotNull(record);

			Assert.IsFalse(record.Error);

			record = _registry.MarkAsFailed(record.Identifier, errorMessage, callStack);

			Assert.NotNull(record);

			Assert.IsTrue(record.Error);

            Assert.IsTrue(record.Dispatched);

			Assert.AreEqual(errorMessage, record.ErrorMessage);

			Assert.AreEqual(callStack, record.CallStack);

			return record;
		}

        public IRecord MarkAsUnableToDispatch()
        {
            var record = CreateRecord(new FakeMessage());

            Assert.NotNull(record);

            record = _registry.GetRecord(record.Identifier);

            Assert.NotNull(record);

            Assert.IsFalse(record.Error);

            record = _registry.MarkAsUnableToDispatch(record.Identifier, true, "Unable to dispatch");

            Assert.NotNull(record);

            Assert.IsTrue(record.Error);

            Assert.IsFalse(record.Dispatched);

            Assert.IsTrue(record.Completed);

            Assert.AreEqual(record.ErrorMessage, "Unable to dispatch");

            return record;
        }


		public void TestThroughputAsynchronously(int howManyMessages, int numberOfThreads)
		{
			var start = DateTime.Now;

			Console.WriteLine("Creating {0} records in the {1} registry", howManyMessages, typeof (FakeMessage).FullName);

			var numberOfLoops = howManyMessages/numberOfThreads + 1;

			for (var i = 0; i < numberOfLoops; i++)
			{
				var results = Parallel.For
					(0, numberOfLoops, x =>
					                   	{
					                   		var record = CreateRecord(new FakeMessage());

					                   		Assert.NotNull(record);
					                   	});
			}

			Console.WriteLine("Created {0} messages in {1} seconds", howManyMessages, DateTime.Now.Subtract(start).TotalSeconds);
		}

		public void TestThroughputSynchronously(int howManyMessages)
		{
			var recordIds = new List<Guid>();

			var start = DateTime.Now;

			Console.WriteLine("Creating {0} records in the {1} registry", howManyMessages, typeof (FakeMessage).FullName);

			for (var i = 0; i < howManyMessages; i++)
			{
				var record = CreateRecord(new FakeMessage());

				Assert.NotNull(record);

				recordIds.Add(record.Identifier);
			}

			Console.WriteLine("Created {0} messages in {1} seconds", howManyMessages, DateTime.Now.Subtract(start).TotalSeconds);

			start = DateTime.Now;

			foreach (var id in recordIds)
			{
				var retrieved = _registry.GetRecord(id);

				Assert.AreEqual(id, retrieved.Identifier);

				Assert.AreEqual(retrieved.MessageType, typeof (FakeMessage));
			}

			Console.WriteLine("Retrieved {0} messages in {1} seconds", howManyMessages, DateTime.Now.Subtract(start).TotalSeconds);
		}
	}
}