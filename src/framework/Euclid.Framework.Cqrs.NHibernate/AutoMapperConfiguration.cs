using System;
using Euclid.Common.Messaging;
using Euclid.Framework.Models;
using FluentNHibernate;
using FluentNHibernate.Automapping;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

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
			return typeof(IReadModel).IsAssignableFrom(type) || typeof(IPublicationRecord).IsAssignableFrom(type);
		}
	}

    public class DefaultStringLengthConvention: IPropertyConvention
    {
        public void Apply(IPropertyInstance instance)
        {
            instance.Length(10000);
        }
    }

}