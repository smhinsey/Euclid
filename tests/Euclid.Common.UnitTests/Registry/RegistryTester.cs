using System;
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

		public IRecord CreateRecord(IMessage message)
		{
			var record = _registry.CreateRecord(message);

			Assert.NotNull(record);

			Assert.AreEqual(message.GetType(), record.MessageType);

			return record;
		}

		public void GetRecord()
		{
			var record = CreateRecord(new FakeMessage());

			Assert.NotNull(record);

			var retrieved = Retrieve(record.Identifier);

			Assert.AreEqual(record.Identifier, retrieved.Identifier);
		}

		public void MarkAsCompleted()
		{
			var record = CreateRecord(new FakeMessage());

			Assert.NotNull(record);

			record = Retrieve(record.Identifier);

			Assert.NotNull(record);

			Assert.IsFalse(record.Completed);

			record = MarkAsCompleted(record.Identifier);

			Assert.NotNull(record);

			Assert.IsTrue(record.Completed);
		}

		public void MarkAsFailed()
		{
			const string errorMessage = "test error message";

			const string callStack = "call stack 1";

			var record = CreateRecord(new FakeMessage());

			Assert.NotNull(record);

			record = Retrieve(record.Identifier);

			Assert.NotNull(record);

			Assert.IsFalse(record.Error);

			record = MarkAsFailed(record.Identifier, errorMessage, callStack);

			Assert.NotNull(record);

			Assert.IsTrue(record.Error);

			Assert.AreEqual(errorMessage, record.ErrorMessage);

			Assert.AreEqual(callStack, record.CallStack);
		}

		public void TestThroughputAsynchronously(int howManyMessages, int numberOfThreads)
		{
			var start = DateTime.Now;

			Console.WriteLine("Creating {0} records in the {1} registry", howManyMessages, typeof (FakeMessage).FullName);

			var numberOfLoops = howManyMessages/numberOfThreads + 1;

			for (var i = 0; i < numberOfLoops; i++)
			{
				var results = Parallel.For(0, numberOfLoops, x =>
				                                             	{
				                                             		var record = CreateRecord(new FakeMessage());

				                                             		Assert.NotNull(record);
				                                             	});
			}

			Console.WriteLine("Created {0} messages in {1} seconds", howManyMessages, DateTime.Now.Subtract(start).TotalSeconds);
		}

		public void TestThroughputSynchronously(int howManyMessages)
		{
			var start = DateTime.Now;

			Console.WriteLine("Creating {0} records in the {1} registry", howManyMessages, typeof (FakeMessage).FullName);

			for (var i = 0; i < howManyMessages; i++)
			{
				var record = CreateRecord(new FakeMessage());

				Assert.NotNull(record);
			}

			Console.WriteLine("Created {0} messages in {1} seconds", howManyMessages, DateTime.Now.Subtract(start).TotalSeconds);
		}

		private IRecord MarkAsCompleted(Guid identifier)
		{
			var record = _registry.MarkAsComplete(identifier);

			Assert.NotNull(record);

			return record;
		}

		private IRecord MarkAsFailed(Guid identifier, string message, string callStack)
		{
			var record = _registry.MarkAsFailed(identifier, message, callStack);

			Assert.NotNull(record);

			return record;
		}

		private IRecord Retrieve(Guid id)
		{
			var record = _registry.Get(id);

			Assert.NotNull(record);

			return record;
		}
	}
}