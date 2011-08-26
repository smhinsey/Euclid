using System;

namespace Euclid.Framework.Models
{
	public interface IInputModel
	{
		string AgentSystemName { get; set; }

		string PartName { get; }

		Type CommandType { get; set; }
	}
}
