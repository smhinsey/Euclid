using System;
using System.IO;
using Euclid.Common.Registry;
using Euclid.Common.TestingFakes.Storage;
using Euclid.Common.Transport;
using FluentNHibernate;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Helpers;
using FluentNHibernate.Conventions.Instances;
using NHibernate.Cfg;
using NHibernate.Linq;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;
using FluentNHibernate.Cfg.Db;
using System.Linq;
namespace Euclid.Common.IntegrationTests.Storage
{
    public class NhibernateRecordRepositoryTest
    {
        [Test]
        public void FirstTest()
        {
            var cfg = new MessageConfiguration();
            var session = Fluently
                .Configure()
                .Database(MsSqlConfiguration.MsSql2008.ConnectionString(c=>c.FromConnectionStringWithKey("test-db")))
                .Mappings(map => map
                                    .AutoMappings.Add(AutoMap.AssemblyOf<FakeMessage>(cfg)
                                    .Conventions.Add(PrimaryKey.Name.Is(primaryKeyName => "Identifier"))))
                .ExposeConfiguration(BuildSchema)
                .BuildSessionFactory()
                .OpenSession();

            Assert.Null(session.Query<FakeMessage>().FirstOrDefault());

            var id = Guid.NewGuid();
            var msg = session.Save(new FakeMessage {Identifier = id}) as FakeMessage;
            Assert.NotNull(msg);
            Assert.AreEqual(id, msg.Identifier);

            msg = session.Query<FakeMessage>().Where(m => m.Identifier == id).FirstOrDefault();
            Assert.NotNull(msg);
            Assert.AreEqual(id, msg.Identifier);

        }

        private static void BuildSchema(Configuration cfg)
        {
            new SchemaExport(cfg).Create(false, true);
        }
    }

    public class MessageConfiguration : DefaultAutomappingConfiguration
    {
        public override bool ShouldMap(Member member)
        {
            var shouldMap = member.DeclaringType.GetInterface(typeof (IRecord).FullName) != null;
            Console.WriteLine("Should map {0}? {1}", member.DeclaringType.Name, shouldMap); 
            
            return shouldMap;
        }

        public override bool IsId(Member member)
        {
					var shouldMap = member.DeclaringType.GetInterface(typeof(IRecord).FullName) != null;
            var isId = (member.Name == "Identifier");

            if (shouldMap)
            {
							Console.WriteLine("Is {0}.{1} the Id field? {2}", member.DeclaringType.Name, member.Name, isId);
            }
            else
            {
							Console.WriteLine("Not mapping {0}.{1}", member.DeclaringType.Name, member.Name);
            }

            return isId;
        }
    }
}
