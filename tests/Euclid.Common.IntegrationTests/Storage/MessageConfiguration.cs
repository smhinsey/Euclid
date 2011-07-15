using System;
using Euclid.Common.TestingFakes.Storage;
using FluentNHibernate;
using FluentNHibernate.Automapping;

namespace Euclid.Common.IntegrationTests.Storage
{
	public class MessageConfiguration : DefaultAutomappingConfiguration
	{
		public override bool IsId(Member member)
		{
			return (member.Name == "Identifier");
		}

		public override bool ShouldMap(Type type)
		{
			return type == typeof (FakeMessage)
			       || type == typeof (FakePublicationRecord);
		}
	}
}