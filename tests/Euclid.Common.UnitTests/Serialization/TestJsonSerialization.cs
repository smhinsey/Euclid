using System;
using System.Collections.Generic;
using System.IO;
using Euclid.Common.Serialization;
using Euclid.Common.TestingFakes.Serialization;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Euclid.Common.UnitTests.Serialization
{
	[TestFixture]
	public class TestJsonSerialization
	{
		[Test]
		public void TestEuclidJsonSerialization()
		{
			var r = new Random((int) DateTime.Now.Ticks);
			var m = new FakeMessage
			        	{
			        		Identifier = Guid.NewGuid(),
			        		CallStack = "flibberty gee",
			        		Dispatched = r.Next()%2 == 0,
			        		Error = r.Next()%2 == 0,
			        		Field1 = new List<string>
			        		         	{
			        		         		r.Next().ToString(),
			        		         		r.Next().ToString(),
			        		         		r.Next().ToString()
			        		         	},
			        		Field2 = r.Next()
			        	};

			var serializer = new JsonMessageSerializer();

			var s = serializer.Serialize(m);

			Assert.NotNull(s);

			var m2 = serializer.Deserialize(s);

			Assert.NotNull(m2);

			Assert.True(m2 is FakeMessage);

			Assert.AreEqual(m.GetType(), m2.GetType());

			Assert.AreEqual(m.Identifier, m2.Identifier);

			Assert.AreEqual(m.Field1, (m2 as FakeMessage).Field1);

			Assert.AreEqual(m.Field2, (m2 as FakeMessage).Field2);
		}

		[Test]
		public void TestJsonNetSerialization()
		{
			var m = new FakeMessage
			        	{
			        		Identifier = Guid.NewGuid(),
			        		Field2 = 3,
			        		Field1 = new List<string> {"foo", "bar", "baz"}
			        	};

			var e = new Envelope(m);

			var serializer = new JsonSerializer();

			serializer.Converters.Insert(0, new EnvelopeConverter());

			var s = JsonConvert.SerializeObject(e);

			var sr = new StringReader(s);

			var r = new JsonTextReader(sr);

			var o = serializer.Deserialize<Envelope>(r);

			Assert.AreEqual(m.GetType(), o.Payload.GetType());

			Assert.AreEqual(m.Identifier, o.Payload.Identifier);

			Assert.AreEqual(m.Field1, (o.Payload as FakeMessage).Field1);

			Assert.AreEqual(m.Field2, (o.Payload as FakeMessage).Field2);
		}
	}
}