using System.Collections.Generic;

namespace ForumComposite.ViewModels.Category
{
	public class CategoryDetailsViewModel
	{
		public string Name { get; set; }

		public IList<ForumAgent.ReadModels.Post> Posts { get; set; }
	}
}