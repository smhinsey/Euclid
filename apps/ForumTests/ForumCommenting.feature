@ForumAgentSpecs
Feature: Forum Commenting
	In order to interact with a Forum
	As a Forum User
	I want to create Posts in that Forum

Scenario: Comment on Post
	Given the agent ForumAgent

	When I publish the command PublishPost
	And the command is complete

	When I publish the command CommentOnPost
	And the command is complete

	Then the query CommentQueries returns the Comment
