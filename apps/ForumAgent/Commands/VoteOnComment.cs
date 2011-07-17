using System;

namespace PublicForm.ForumAgent.Commands
{
	public class VoteOnComment
	{
		public Guid AuthorIdentifier { get; set; }
		public Guid PostIdentifier { get; set; }
		public bool VoteUp { get; set; } 
	}
}