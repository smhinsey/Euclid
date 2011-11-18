using System;

namespace ForumAgent.Processors
{
	public class PostNotFoundException : Exception
	{
		public PostNotFoundException(string message) : base(message)
		{
		}
	}
}