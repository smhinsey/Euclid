using System.Collections.Generic;

namespace ForumComposite.ViewModels.Category
{
	public class AllCategoriesViewModel
	{
		public IList<ForumAgent.ReadModels.CategoryDetail> Categories { get; set; }
	}
}