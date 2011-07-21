using System;

namespace Euclid.Agent
{
	[AttributeUsage(AttributeTargets.Assembly)]
	public class AgentNameAttribute : Attribute
	{
		public AgentNameAttribute(string name)
		{
			Name = name;
		}

		public string Name { get; set; }
	}
}