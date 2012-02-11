using System;

namespace ForumAgent
{
	public class CommentNotFoundException : Exception
	{
		public CommentNotFoundException(string message) : base(message)
		{ }
	}
}