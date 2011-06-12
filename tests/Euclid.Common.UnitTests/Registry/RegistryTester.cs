using System;
using Euclid.Common.Registry;
using Euclid.Common.TestingFakes.Registry;
using NUnit.Framework;

namespace Euclid.Common.UnitTests.Registry
{
    public class RegistryTester<T>
        where T : ITestRegistry
    {
        private readonly T _registry;

        public RegistryTester(T registry)
        {
            _registry = registry;
        }

        public IRecord<FakeMessage> CreateRecord(FakeMessage message)
        {
            var record = _registry.Registry.CreateRecord(message);

            Assert.NotNull(record);

            Assert.NotNull(record.Message);

            Assert.AreEqual(message.GetType(), record.Message.GetType());

            return record;
        }

        public IRecord<FakeMessage> Retrieve(Guid id)
        {
            var record = _registry.Registry.Get(id);

            Assert.NotNull(record);

            return record;
        }

        public IRecord<FakeMessage> MarkAsCompleted(Guid identifier)
        {
            var record = _registry.Registry.MarkAsComplete(identifier);

            Assert.NotNull(record);

            return record;
        }

        public IRecord<FakeMessage> MarkAsFailed(Guid identifier, string message, string callStack)
        {
            var record = _registry.Registry.MarkAsFailed(identifier, message, callStack);

            Assert.NotNull(record);

            return record;
        }
    }
}