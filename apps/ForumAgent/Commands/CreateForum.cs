using Euclid.Framework.Cqrs;

namespace ForumAgent.Commands
{
	public class CreateForum : DefaultCommand
	{
		public string Name { get; set; }

		public string UrlHostName { get; set; }

		public string UrlSlug { get; set; }
	}
}