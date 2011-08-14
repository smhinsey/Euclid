using System;

namespace Euclid.Framework.Agent.Attributes
{
	[AttributeUsage(AttributeTargets.Assembly)]
	public abstract class TextAttribute : Attribute, IAgentAttribute
	{
		public string Value { get; set; }
	}
}