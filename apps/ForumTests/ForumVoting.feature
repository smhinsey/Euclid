@ForumAgentSpecs
Feature: Forum Voting
	In order to interact with a Forum
	As a Forum User
	I want to vote on Posts and Comments in that Forum

Scenario: Vote Post Up
	Given the agent ForumAgent

	When I publish the command PublishPost:
	| Title        | Body      | CategoryIdentifier                   | AuthorIdentifier                     | ForumIdentifier                      |
	| Vote up post | Post Body | 11111111-1111-1111-1111-111111111111 | 00000000-0000-0000-0000-000000000000 | 22222222-2222-2222-2222-222222222222 |
	And the command is complete

	When I publish the command VoteOnPost:
	| AuthorIdentifier                     | VoteUp |
	| 00000000-0000-0000-0000-000000000000 | true   |
	And the command is complete

	Then the Post has a score of 1

Scenario: Vote Post Down
	Given the agent ForumAgent

	When I publish the command PublishPost:
	| Title          | Body      | CategoryIdentifier                   | AuthorIdentifier                     | ForumIdentifier                      |
	| Vote Post Down | Post Body | 11111111-1111-1111-1111-111111111111 | 00000000-0000-0000-0000-000000000000 | 22222222-2222-2222-2222-222222222222 |
	And the command is complete

	When I publish the command VoteOnPost:
	| AuthorIdentifier                     | VoteUp |
	| 00000000-0000-0000-0000-000000000000 | false  |
	And the command is complete

	Then the Post has a score of -1

Scenario: Vote Comment Up
	Given the agent ForumAgent

	When I publish the command PublishPost:
	| Title      | Body      | CategoryIdentifier                   | AuthorIdentifier                     | ForumIdentifier                      |
	| Post Title | Post Body | 11111111-1111-1111-1111-111111111111 | 00000000-0000-0000-0000-000000000000 | A96DA8B2-7E80-4424-83B4-A059C62FE898 |
	And the command is complete

	When I publish the command CommentOnPost:
	| Title      | Body         | AuthorIdentifier                     | ForumIdentifier                      |
	| Comment Up | Comment Body | 00000000-0000-0000-0000-000000000000 | E061E032-3402-4296-B09F-6F6EA91AB484 |
	And the command is complete
	
	When I publish the command VoteOnComment:
	| AuthorIdentifier                     | VoteUp |
	| 00000000-0000-0000-0000-000000000000 | true   |
	And the command is complete

	Then the Comment has a score of 1

Scenario: Vote Comment Down
	Given the agent ForumAgent

	When I publish the command PublishPost:
	| Title      | Body      | CategoryIdentifier                   | AuthorIdentifier                     | ForumIdentifier                      |
	| Post Title | Post Body | 11111111-1111-1111-1111-111111111111 | 00000000-0000-0000-0000-000000000000 | 22222222-2222-2222-2222-222222222222 |
	And the command is complete

	When I publish the command CommentOnPost:
	| Title        | Body         | AuthorIdentifier                     | ForumIdentifier                      |
	| Comment Down | Comment Body | 00000000-0000-0000-0000-000000000000 | 22222222-2222-2222-2222-222222222222 |
	And the command is complete
	
	When I publish the command VoteOnComment:
	| AuthorIdentifier                     | VoteUp |
	| 00000000-0000-0000-0000-000000000000 | false  |
	And the command is complete

	Then the Comment has a score of -1