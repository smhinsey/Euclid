@ForumAgentSpecs
Feature: Forum Posting
	In order to interact with a Forum
	As a Forum User
	I want to create Posts in that Forum

Scenario: Publish Post
	Given a runtime fabric for agent ForumAgent
	
	And I publish the command PublishPost
	When the command is complete
	
	Then the query ForumQueries returns the Post

Scenario: Publish Post in a Category
	Given a runtime fabric for agent ForumAgent
	
	And I publish the command PublishPost
	When the command is complete
	
	Then the query CategoryQueries returns Post
