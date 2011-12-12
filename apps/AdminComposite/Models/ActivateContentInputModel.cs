using System;
using Euclid.Composites.Mvc.Models;
using ForumAgent.Commands;

namespace AdminComposite.Models
{
	public class ActivateContentInputModel : DefaultInputModel
	{
		public ActivateContentInputModel()
		{
			CommandType = typeof (ActivateContent);
		}

		public Guid ContentIdentifier { get; set; }
		public bool Active { get; set; }
	}
}