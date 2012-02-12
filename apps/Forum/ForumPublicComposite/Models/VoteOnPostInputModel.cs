using System;
using Euclid.Composites.Mvc.Models;
using ForumAgent.Commands;

namespace ForumComposite.Models
{
	public class VoteOnPostInputModel : DefaultInputModel
	{
		public VoteOnPostInputModel()
		{
			CommandType = typeof(VoteOnPost);
		}

		public Guid AuthorIdentifier { get; set; }

		public Guid PostIdentifier { get; set; }

		public bool VoteUp { get; set; }
	}
}