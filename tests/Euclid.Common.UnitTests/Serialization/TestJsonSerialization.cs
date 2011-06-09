using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Euclid.Common.Serialization;
using Euclid.Common.Transport;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NUnit.Framework;

namespace Euclid.Common.UnitTests.Serialization
{
    [TestFixture]
    public class TestJsonSerialization
    {
        [Test]
        public void TestJsonNetSerialization()
        {
            var m = new Message
                        {
                            Identifier = Guid.NewGuid(),
                            Field2 = 3,
                            Field1 = new List<string> {"foo", "bar", "baz"}
                        };

            var e = new Envelope(m);

            var serializer = new Newtonsoft.Json.JsonSerializer();

            serializer.Converters.Insert(0, new EnvelopeConverter());

            var s = JsonConvert.SerializeObject(e);

            var sr = new StringReader(s);
            
            var r = new JsonTextReader(sr);   
            
            var o = serializer.Deserialize<Envelope>(r);

            Assert.AreEqual(m.GetType(), o.Message.GetType());

            Assert.AreEqual(m.Identifier, o.Message.Identifier);

            Assert.AreEqual(m.Field1, (o.Message as Message).Field1);

            Assert.AreEqual(m.Field2, (o.Message as Message).Field2);
        }

        [Test]
        public void TestEuclidJsonSerialization()
        {
            var r = new Random((int)DateTime.Now.Ticks);
            var m = new Message
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

            Assert.True(m2 is Message);

            Assert.AreEqual(m.GetType(), m2.GetType());

            Assert.AreEqual(m.Identifier, m2.Identifier);

            Assert.AreEqual(m.Field1, (m2 as Message).Field1);

            Assert.AreEqual(m.Field2, (m2 as Message).Field2);

        }
    }



    public class Message : IMessage
    {
        public string CallStack { get; set; }
        public bool Dispatched { get; set; }
        public bool Error { get; set; }
        public string ErrorMessage { get; set; }
        public Guid Identifier { get; set; }

        public IList<string> Field1 { get; set; }
        public int Field2 { get; set; }
    }

   

}
