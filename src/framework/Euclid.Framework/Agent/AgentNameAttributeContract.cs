using System;

namespace Euclid.Framework.Agent
{
	public abstract class AgentNameAttributeContract : Attribute
	{
		public abstract string Value { get; set; }
	}
}