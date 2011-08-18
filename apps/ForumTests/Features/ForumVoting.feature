@AgentFeature
Feature: Forum Voting
	In order to interact with a Forum
	As a Forum User
	I want to vote on Posts and Comments in that Forum

Scenario: Vote on Post
	Given a runtime fabric for agent ForumAgent

	And I publish the comand PublishPost
	When the command is complete

	And I publish the comand VoteOnPost
	When the command is complete

	Then the query ForumQueries returns the Score


Scenario: Vote on Comment
	Given a runtime fabric for agent ForumAgent
	
	And I publish the comand PublishPost
	When the command is complete

	And I publish the comand CommentOnPost
	When the command is complete
	
	And I publish the comand VoteOnComment
	When the command is complete

	Then the query CommentQueries returns the Score
