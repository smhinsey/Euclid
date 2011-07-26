using System;

namespace Euclid.SDK.TestingFakes.Composites
{
	[AttributeUsage(AttributeTargets.Property)]
	public class FakeAttribute : Attribute
	{
		public string Description { get; set; }
	}
}