using System;
using Euclid.Composites.Mvc.Models;
using Euclid.Framework.AgentMetadata.Extensions;
using ForumAgent.Commands;

namespace AdminComposite.Models
{
	public class CreateCategoryInputModel : DefaultInputModel
	{
		public CreateCategoryInputModel()
		{
			CommandType = typeof (CreateCategory);
		}

		public string Name { get; set; }
		public string Slug { get; set; }
		public Guid ForumIdentifier { get; set; }
		public bool Active { get; set; }
		public Guid CreatedBy { get; set; }
	}
}