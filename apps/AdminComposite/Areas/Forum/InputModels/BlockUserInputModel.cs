using System;
using Euclid.Composites.Mvc.Models;
using ForumAgent.Commands;

namespace AdminComposite.Areas.Forum.InputModels
{
	public class BlockUserInputModel : DefaultInputModel
	{
		public BlockUserInputModel()
		{
			CommandType = typeof (BlockUser);
		}

		public Guid UserIdentifier { get; set; }
	}
}