using System;
using Euclid.Composites.Mvc.Models;
using ForumAgent.Commands;

namespace AdminComposite.Models
{
	public class UnblockUserInputModel : DefaultInputModel
	{
		public UnblockUserInputModel()
		{
			CommandType = typeof (UnblockUser);
		}

		public Guid UserIdentifier { get; set; }
	}
}