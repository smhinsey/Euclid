﻿namespace ForumComposite.ViewModels.Category
{
	public class CategoryDetailsViewModel
	{
		public string Name { get; set; }

		public ForumAgent.ReadModels.PostListing Posts { get; set; }
	}
}