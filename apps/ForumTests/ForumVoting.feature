@ForumAgentSpecs
Feature: Forum Voting
	In order to interact with a Forum
	As a Forum User
	I want to vote on Posts and Comments in that Forum

Scenario: Vote Post Up
	Given the agent ForumAgent

	When I publish the command PublishPost
	And the command is complete

	When I publish the command VoteOnPost VoteUp=true
	And the command is complete

	Then the query ForumQueries returns the post with a score of 1

Scenario: Vote Post Down
	Given the agent ForumAgent

	When I publish the command PublishPost
	And the command is complete

	When I publish the command VoteOnPost VoteUp=false
	And the command is complete

	Then the query ForumQueries returns the post with a score of -1

Scenario: Vote Comment Up
	Given the agent ForumAgent
	
	When I publish the command PublishPost
	And the command is complete

	When I publish the command CommentOnPost
	And the command is complete
	
	When I publish the command VoteOnComment VoteUp=true
	And the command is complete

	Then the query CommentQueries returns the post with a score of 1

Scenario: Vote Comment Down
	Given the agent ForumAgent
	
	When I publish the command PublishPost
	And the command is complete

	When I publish the command CommentOnPost
	And the command is complete
	
	When I publish the command VoteOnComment VoteUp=false
	And the command is complete

	Then the query CommentQueries returns the post with a score of -1
