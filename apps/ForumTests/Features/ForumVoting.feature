@AgentFeature
Feature: Forum Voting
	In order to interact with a Forum
	As a Forum User
	I want to vote on Posts and Comments in that Forum

Scenario: Vote on Post
	Given a runtime fabric for agent ForumAgent

	And I publish the command PublishPost
	When the command is complete

	And I publish the command VoteOnPost
	When the command is complete

	Then the query ForumQueries returns the Score


Scenario: Vote on Comment
	Given a runtime fabric for agent ForumAgent
	
	And I publish the command PublishPost
	When the command is complete

	And I publish the command CommentOnPost
	When the command is complete
	
	And I publish the command VoteOnComment
	When the command is complete

	Then the query CommentQueries returns the Score
