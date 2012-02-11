using System;

namespace ForumAgent
{
	public class CategoryNotFoundException : Exception
	{
		public CategoryNotFoundException(string message)  : base(message)
		{
		}
	}
}