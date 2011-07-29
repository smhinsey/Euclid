using System;
using Euclid.Common.TestingFakes.Storage;
using Euclid.Common.TestingFakes.Storage.Model;
using FluentNHibernate;
using FluentNHibernate.Automapping;

namespace Euclid.Common.IntegrationTests
{
	public class AutoMapperConfiguration : DefaultAutomappingConfiguration
	{
		public override bool IsId(Member member)
		{
			return (member.Name == "Identifier");
		}

		public override bool ShouldMap(Type type)
		{
			return type == typeof (FakeMessage)
			       || type == typeof (FakePublicationRecord) || type == typeof (FakeModel);
		}
	}
}