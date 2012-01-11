namespace ForumComposite.ViewModels.Tag
{
	public class TagDetailViewModel
	{
		public string Name { get; set; }

		public int CurrentPage { get; set; }

		public ForumAgent.ReadModels.PostListing Listing { get; set; }
	}
}