@ForumAgentSpecs
Feature: Forum Posting
	In order to interact with a Forum
	As a Forum User
	I want to create Posts in that Forum

Scenario: Publish Post
	Given the agent ForumAgent
	
	When I publish the command PublishPost:
	| Title      | Body      | CategoryIdentifier                   | AuthorIdentifier                     | ForumIdentifier                      |
	| Post Title | Post Body | 11111111-1111-1111-1111-111111111111 | 00000000-0000-0000-0000-000000000000 | 33333333-3333-3333-3333-333333333333 |
	And the command is complete
	
	Then run FindByTitle on PostQueries with:
	| Title      |
	| Post Title |

	And the Post has values:
	| Title      | Body      | CategoryIdentifier                   | AuthorIdentifier                     | ForumIdentifier                      | AuthorDisplayName | CommentCount |
	| Post Title | Post Body | 11111111-1111-1111-1111-111111111111 | 00000000-0000-0000-0000-000000000000 | 33333333-3333-3333-3333-333333333333 | Anonymous         | 0            |

Scenario: Publish Post in a Category
	Given the agent ForumAgent
	
	When I publish the command PublishPost:
	| Title      | Body      | CategoryIdentifier                   | AuthorIdentifier                     | ForumIdentifier                      |
	| Post Title | Post Body | 11111111-1111-1111-1111-111111111111 | 00000000-0000-0000-0000-000000000000 | 33333333-3333-3333-3333-333333333333 |
	And the command is complete
	
	Then retrieve a List of Post by running FindPostsByCategory on PostQueries with:
	| CategoryIdentifier                   |
	| 11111111-1111-1111-1111-111111111111 |

	And the resulting list contains a Post with values:
	| Title      | Body      | CategoryIdentifier                   | AuthorIdentifier                     | ForumIdentifier                      | AuthorDisplayName | CommentCount |
	| Post Title | Post Body | 11111111-1111-1111-1111-111111111111 | 00000000-0000-0000-0000-000000000000 | 33333333-3333-3333-3333-333333333333 | Anonymous         | 0            |
