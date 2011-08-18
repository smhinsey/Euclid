@ForumAgentSpecs
Feature: User Profiles
	In order to interact with a Forum
	As a Forum User
	I want to create and maintain a Profile

Scenario: Register a Profile
	Given the agent ForumAgent

	When I publish the command RegisterUser
	And the command is complete

	Then the query UserQueries returns the Profile


Scenario: Update a Profile
	Given the agent ForumAgent

	When I publish the command RegisterUser
	And the command is complete

	When I publish the command UpdateUserProfile
	And the command is complete

	Then the query UserQueries returns the updated Profile

Scenario: Authenticate as User
	Given the agent ForumAgent

	When I publish the command RegisterUser
	And the command is complete

	Then the query UserQueries can authenticate