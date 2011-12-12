using System;
using Euclid.Composites.Mvc.Models;
using ForumAgent.Commands;

namespace AdminComposite.Models
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