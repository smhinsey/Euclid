@ForumAgentSpecs
Feature: User Profiles
	In order to interact with a Forum
	As a Forum User
	I want to create and maintain a Profile

Scenario: Register a Profile
	Given a runtime fabric for agent ForumAgent

	And I publish the command RegisterUser
	When the command is complete

	Then the query UserQueries returns the Profile


Scenario: Update a Profile
	Given a runtime fabric for agent ForumAgent

	And I publish the command RegisterUser
	When the command is complete

	And I publish the command UpdateUserProfile
	When the command is complete

	Then the query UserQueries returns the updated Profile

Scenario: Authenticate as User
	Given a runtime fabric for agent ForumAgent

	And I publish the command RegisterUser
	When the command is complete

	Then the query UserQueries can authenticate