using System;

namespace ForumAgent
{
	public class TagNotFoundException : Exception
	{
		public TagNotFoundException(string message)  : base(message)
		{
		}
	}
}