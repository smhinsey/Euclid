using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace Euclid.Framework.Cqrs.NHibernate
{
	public class DefaultStringLengthConvention : IPropertyConvention
	{
		public void Accept(IAcceptanceCriteria<IPropertyInspector> criteria)
		{
			criteria.Expect(x => x.Type == typeof(string)).Expect(x => x.Length == 0);
		}

		public void Apply(IPropertyInstance instance)
		{
			instance.Length(10000);
		}
	}
}