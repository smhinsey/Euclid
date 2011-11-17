using System;
using Euclid.Composites.Mvc.Models;
using Euclid.Framework.AgentMetadata.Extensions;
using ForumAgent.Commands;

namespace AdminComposite.Models
{
	public class UpdateCategoryInputModel : DefaultInputModel
	{
		public UpdateCategoryInputModel()
		{
			CommandType = typeof (UpdateCategory);
			AgentSystemName = CommandType.Assembly.GetAgentMetadata().SystemName;
		}

		public Guid Identifier { get; set; }
		public bool Active { get; set; }
		public string Name { get; set; }
	}
}