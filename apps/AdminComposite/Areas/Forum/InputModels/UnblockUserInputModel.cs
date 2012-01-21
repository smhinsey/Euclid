using System;
using Euclid.Composites.Mvc.Models;
using ForumAgent.Commands;

namespace AdminComposite.Areas.Forum.InputModels
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