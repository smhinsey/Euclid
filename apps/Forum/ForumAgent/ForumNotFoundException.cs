using System;

namespace ForumAgent
{
	public class ForumNotFoundException : Exception
	{
		public ForumNotFoundException(string message) : base(message)
		{
		}
	}
}