using System;
using Euclid.Composites.Mvc.Models;

namespace ForumComposite.Models
{
	public class VoteOnPostInputModel : InputModelBase
	{
		public Guid AuthorIdentifier { get; set; }

		public Guid PostIdentifier { get; set; }

		public bool VoteUp { get; set; }
	}
}