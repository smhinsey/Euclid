@ForumAgentSpecs
Feature: Forum Voting
	In order to interact with a Forum
	As a Forum User
	I want to vote on Posts and Comments in that Forum

Scenario: Vote Post Up
	Given a runtime fabric for agent ForumAgent

	And I publish the command PublishPost
	When the command is complete

	And I publish the command VoteOnPost VoteUp=true
	When the command is complete

	Then the query ForumQueries returns the post with a score of 1

Scenario: Vote Post Down
	Given a runtime fabric for agent ForumAgent

	And I publish the command PublishPost
	When the command is complete

	And I publish the command VoteOnPost VoteUp=false
	When the command is complete

	Then the query ForumQueries returns the post with a score of -1

Scenario: Vote Comment Up
	Given a runtime fabric for agent ForumAgent
	
	And I publish the command PublishPost
	When the command is complete

	And I publish the command CommentOnPost
	When the command is complete
	
	And I publish the command VoteOnComment VoteUp=true
	When the command is complete

	Then the query CommentQueries returns the post with a score of 1

Scenario: Vote Comment Down
	Given a runtime fabric for agent ForumAgent
	
	And I publish the command PublishPost
	When the command is complete

	And I publish the command CommentOnPost
	When the command is complete
	
	And I publish the command VoteOnComment VoteUp=false
	When the command is complete

	Then the query CommentQueries returns the post with a score of -1
