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
		}

		public Guid CategoryIdentifier { get; set; }
		public bool Active { get; set; }
		public string Name { get; set; }
		public string Slug { get; set; }
	}
}