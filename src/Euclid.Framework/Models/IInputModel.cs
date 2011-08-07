using System;

namespace Euclid.Framework.Models
{
	public interface IInputModel
	{
		string AgentSystemName { get; set; }
		string CommandName { get; }
		Type CommandType { get; set; }
	}
}