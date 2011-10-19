namespace Euclid.Framework.TestingFakes.EventSourcing.ReadModel
{
	public class PostListing
	{
		public virtual string Title { get; set; }
		public virtual string Body { get; set; }
		public virtual string AuthorUsername { get; set; }
		public virtual int Score { get; set; }
	}
}