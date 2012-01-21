using System;
using Euclid.Composites.Mvc.Models;
using ForumAgent.Commands;

namespace AdminComposite.Areas.Forum.InputModels
{
	public class ActivateCategoryInputModel : DefaultInputModel
	{
		public ActivateCategoryInputModel()
		{
			CommandType = typeof (ActivateCategory);
		}

		public Guid CategoryIdentifier { get; set; }
		public bool Active { get; set; }
	}
}