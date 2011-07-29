using System;

namespace Euclid.Agent
{
	[AttributeUsage(AttributeTargets.Assembly)]
	public abstract class TextAttribute : Attribute, IAgentAttribute
	{
		public string Value { get; set; }
	}
}