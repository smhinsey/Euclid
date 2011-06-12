using System;
using System.Threading.Tasks;
using Euclid.Common.Registry;
using Euclid.Common.Storage;
using Euclid.Common.TestingFakes.Registry;
using Euclid.Common.Transport;
using NUnit.Framework;

namespace Euclid.Common.UnitTests.Registry
{
    [TestFixture]
    public class InMemoryRegistryTests : ITestRegistry
    {
        private RegistryTester<InMemoryRegistryTests> _registryTester;
        private readonly IBasicRecordRepository<FakeRecord,FakeMessage> _recordRepository = new InMemoryRecordRepository<FakeRecord,FakeMessage>();

        [TestFixtureSetUp]
        public void SetupTest()
        {
            _registryTester = new RegistryTester<InMemoryRegistryTests>(this);
        }

        [Test]
        public void CreateRecord()
        {
            _registryTester.CreateRecord(new FakeMessage());
        }

        [Test]
        public void GetRecord()
        {
            var record = _registryTester.CreateRecord(new FakeMessage());
            
            Assert.NotNull(record);

            var retrieved = _registryTester.Retrieve(record.Identifier);

            Assert.AreEqual(record.Identifier, retrieved.Identifier);
        }

        [Test]
        public void MarkAsCompleted()
        {
            var record = _registryTester.CreateRecord(new FakeMessage());

            Assert.NotNull(record);

            record = _registryTester.Retrieve(record.Identifier);

            Assert.NotNull(record);

            Assert.IsFalse(record.Completed);

            record = _registryTester.MarkAsCompleted(record.Identifier);

            Assert.NotNull(record);

            Assert.IsTrue(record.Completed);
        }

        [Test]
        public void MarkAsFailed()
        {
            const string errorMessage = "test error message";

            const string callStack = "call stack 1";

            var record = _registryTester.CreateRecord(new FakeMessage());

            Assert.NotNull(record);

            record = _registryTester.Retrieve(record.Identifier);

            Assert.NotNull(record);

            Assert.IsFalse(record.Error);

            record = _registryTester.MarkAsFailed(record.Identifier, errorMessage, callStack);

            Assert.NotNull(record);

            Assert.IsTrue(record.Error);

            Assert.AreEqual(errorMessage, record.ErrorMessage);

            Assert.AreEqual(callStack, record.CallStack);
        }

        private const int LargeNumber = 10000;
        private const int NumberThreads = 15;
        
        [Test]
        public void TestThroughputSynchronously()
        {
            var start = DateTime.Now;

            Console.WriteLine("Creating {0} records in the {1} registry", LargeNumber, _recordRepository.GetType().Name);

            for (var i = 0; i < LargeNumber; i++)
            {
                var record = _registryTester.CreateRecord(new FakeMessage());

                Assert.NotNull(record);
            }

            Console.WriteLine("Created {0} messages in {1} seconds", LargeNumber, DateTime.Now.Subtract(start).TotalSeconds);
        }

        [Test]
        public void TestThroughputAsynchronously()
        {
            var start = DateTime.Now;

            Console.WriteLine("Creating {0} records in the {1} registry", LargeNumber, _recordRepository.GetType().Name);

            const int numberOfLoops = LargeNumber/NumberThreads + 1;

            for (var i = 0; i < numberOfLoops; i++)
            {
                var results = Parallel.For(0, numberOfLoops, x =>
                                                                 {
                                                                     var record = _registryTester.CreateRecord(new FakeMessage());

                                                                     Assert.NotNull(record);
                                                                 });
            }

            Console.WriteLine("Created {0} messages in {1} seconds", LargeNumber, DateTime.Now.Subtract(start).TotalSeconds);
        }

        public IRegistry<FakeRecord, FakeMessage> Registry
        {
            get { return new InMemoryRegistry<FakeRecord, FakeMessage>(_recordRepository); }
        }
    }
}