using System;

namespace ForumAgent
{
	public class UserNotFoundException : Exception
	{
		public UserNotFoundException(string name)
			: base(name)
		{
		}

		public UserNotFoundException(Guid id)
			: base(id.ToString())
		{
		}
	}
}