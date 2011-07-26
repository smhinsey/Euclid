using System;

namespace Euclid.Agent
{
	[AttributeUsage(AttributeTargets.Assembly)]
	public abstract class TextAttribute : Attribute, IAgentAttribute
	{
		public string Value { get; set; }
	}

	public class AgentNameAttribute : TextAttribute
	{
	}

	public class AgentSystemNameAttribute : TextAttribute
	{
	}

	public class AgentSchemeAttribute : TextAttribute
	{
	}
}