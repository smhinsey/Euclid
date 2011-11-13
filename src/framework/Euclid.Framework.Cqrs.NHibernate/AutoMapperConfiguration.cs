using System;
using Euclid.Common.Messaging;
using Euclid.Framework.Models;
using FluentNHibernate;
using FluentNHibernate.Automapping;

namespace Euclid.Framework.Cqrs.NHibernate
{
	public class AutoMapperConfiguration : DefaultAutomappingConfiguration
	{
		public override bool IsId(Member member)
		{
			return member.Name == "Identifier";
		}

		public override bool ShouldMap(Type type)
		{
			return (typeof(IReadModel).IsAssignableFrom(type) || typeof(IPublicationRecord).IsAssignableFrom(type)) && type.BaseType != typeof(UnpersistedReadModel);
		}
	}
}