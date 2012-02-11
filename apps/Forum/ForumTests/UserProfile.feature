@ForumAgentSpecs
Feature: User Profiles
	In order to interact with a Forum
	As a Forum User
	I want to create and maintain a Profile

Scenario: Register a Profile
	Given the agent ForumAgent

	When I publish the command RegisterUser:
	| Username | PasswordHash | PasswordSalt | ForumIdentifier                      |
	| johndoe  | hash         | salt         | 22222222-2222-2222-2222-222222222222 |
	And the command is complete

	Then run FindByUsername on UserQueries with:
	| Username |
	| johndoe  |
	
	Then the User has values:
	| Username | PasswordHash | PasswordSalt | ForumIdentifier                      |
	| johndoe  | hash         | salt         | 22222222-2222-2222-2222-222222222222 |


Scenario: Update a Profile
	Given the agent ForumAgent

	When I publish the command RegisterUser:
	| Username | PasswordHash | PasswordSalt | ForumIdentifier                      |
	| johndoe  | hash         | salt         | 22222222-2222-2222-2222-222222222222 |
	And the command is complete

	When I publish the command UpdateUserProfile:
	| Email               | AvatarUrl             |
	| johndoe@doejohn.com | http://avatar/johndoe |
	And the command is complete

	Then run UserProfileByUsername on UserQueries with:
	| Username |
	| johndoe  |

	And the UserProfile has values:
	| Email               | AvatarUrl             |
	| johndoe@doejohn.com | http://avatar/johndoe |

Scenario: Authenticate as User
	Given the agent ForumAgent

	When I publish the command RegisterUser:
	| Username | PasswordHash | PasswordSalt | ForumIdentifier                      |
	| johndoe  | hash         | hash         | 22222222-2222-2222-2222-222222222222 |
	And the command is complete

	Then running Authenticate on  UserQueries will return true