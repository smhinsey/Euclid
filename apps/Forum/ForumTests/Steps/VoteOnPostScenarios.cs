using Euclid.TestingSupport;
using ForumAgent.Commands;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace ForumTests.Steps
{
	[Binding]
	[StepScope(Scenario = "Vote Post Up")]
	[StepScope(Scenario = "Vote Post Down")]
	public class VoteOnPostScenarios : PublishPostSpecification, ICommandPublishStep<VoteOnPost>
	{
		public VoteOnPost GetCommand(VoteOnPost command)
		{
			command.PostIdentifier = PostIdentifier;

			return command;
		}

		[Then(@"the Post has a score of (.*)")]
		public void VerifyPostScore(int expectedScore)
		{
			var post = PostQueries.FindById(PostIdentifier);

			Assert.AreEqual(expectedScore, post.Score);
		}
	}
}