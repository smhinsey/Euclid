using System;
using Euclid.Composites.Mvc.Models;
using ForumAgent.Commands;

namespace AdminComposite.Areas.Forum.InputModels
{
	public class UpdateTagInputModel : DefaultInputModel
	{
		public UpdateTagInputModel()
		{
			CommandType = typeof(UpdateTag);
		}

		public bool Active { get; set; }

		public string Name { get; set; }

		public Guid TagIdentifier { get; set; }
	}
}