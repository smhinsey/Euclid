using System;
using Euclid.Composites.Mvc.Models;
using ForumAgent.Commands;

namespace AdminComposite.Models
{
	public class UpdateTagInputModel : DefaultInputModel
	{
		public UpdateTagInputModel()
		{
			CommandType = typeof(UpdateTag);
		}

		public bool Active { get; set; }

		public string Name { get; set; }

		public string Slug { get; set; }

		public Guid TagIdentifier { get; set; }
	}
}