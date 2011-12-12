using System;
using Euclid.Composites.Mvc.Models;
using ForumAgent.Commands;

namespace AdminComposite.Models
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