using System;

namespace Euclid.Framework.Agent
{
	public abstract class AgentSystemNameAttributeContract : Attribute
	{
		public abstract string Value { get; set; }
	}
}