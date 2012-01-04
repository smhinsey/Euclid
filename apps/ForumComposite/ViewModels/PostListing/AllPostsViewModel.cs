namespace ForumComposite.ViewModels.PostListing
{
	public class AllPostsViewModel
	{
		public int CurrentPage { get; set; }
		public ForumAgent.ReadModels.PostListing Listing { get; set; }
	}
}