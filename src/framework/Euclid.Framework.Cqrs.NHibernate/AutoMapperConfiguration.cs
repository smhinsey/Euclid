using System;
using Euclid.Framework.Models;
using FluentNHibernate;
using FluentNHibernate.Automapping;

namespace Euclid.Framework.Cqrs.NHibernate
{
	public class AutoMapperConfiguration : DefaultAutomappingConfiguration
	{
		public override bool IsId(Member member)
		{
			return (member.Name == "Identifier");
		}

		public override bool ShouldMap(Type type)
		{
			return typeof(IReadModel).IsAssignableFrom(type);
		}
	}
}