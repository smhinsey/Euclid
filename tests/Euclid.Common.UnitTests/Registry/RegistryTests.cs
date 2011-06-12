using Euclid.Common.TestingFakes.Registry;
using NUnit.Framework;

namespace Euclid.Common.UnitTests.Registry
{
	[TestFixture]
	public class RegistryTests
	{
		[Test]
		public void SaveRecord()
		{
			var f = new FakeRegistry();
			var r = new FakeRecord();

			f.Add(r);
			var r2 = f.GetCurrentRecord(r.Identifier);

			Assert.NotNull(r2);
			Assert.AreEqual(r2.Identifier, r.Identifier);
		}
	}
}