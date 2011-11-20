using System;

namespace ForumAgent
{
	public class PostNotFoundException : Exception
	{
		public PostNotFoundException(string message) : base(message)
		{
		}
	}
}