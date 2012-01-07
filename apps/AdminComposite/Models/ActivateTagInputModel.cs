using System;
using Euclid.Composites.Mvc.Models;
using ForumAgent.Commands;

namespace AdminComposite.Models
{
	public class ActivateTagInputModel : DefaultInputModel
	{
		public ActivateTagInputModel()
		{
			CommandType = typeof (ActivateTag);
		}

		public Guid TagIdentifier { get; set; }
		public bool Active { get; set; }
	}
}