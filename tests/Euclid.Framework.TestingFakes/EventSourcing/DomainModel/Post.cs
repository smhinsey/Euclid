namespace Euclid.Framework.TestingFakes.EventSourcing.DomainModel
{
	public class Post
	{
		public User Author { get; set; }

		public string Body { get; set; }

		public int Score { get; set; }

		public string Title { get; set; }
	}
}