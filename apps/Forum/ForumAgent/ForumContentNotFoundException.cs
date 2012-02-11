using System;

namespace ForumAgent
{
	public class ForumContentNotFoundException : Exception
	{
		public ForumContentNotFoundException(string message) : base(message)
		{
		}
	}
}