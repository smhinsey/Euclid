using System;

namespace ForumAgent
{
	public class ContentNotFoundException : Exception
	{
		public ContentNotFoundException(string message) :base(message)
		{
		}
	}
}