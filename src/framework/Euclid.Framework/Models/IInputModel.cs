using System;

namespace Euclid.Framework.Models
{
	public interface IInputModel
	{
		string AgentSystemName { get; set; }

		Type CommandType { get; set; }

		string PartName { get; }
	}
}