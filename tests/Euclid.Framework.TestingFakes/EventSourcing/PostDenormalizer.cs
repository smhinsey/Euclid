namespace Euclid.Framework.TestingFakes.EventSourcing
{
	// SELF: this should exist as a series of denormalizers. e.g., PostToPostListing, PostToUserProfile, etc.
	// this specific connection between the denormalizer and the resultant read model will discourage authors
	// from writing overcomplex denormalizers or those which share responsibilities associated with multiple
	// read models.
	public class PostDenormalizer
	{
	}
}