﻿@AgentFeature
Feature: Forum Commenting
	In order to interact with a Forum
	As a Forum User
	I want to create Posts in that Forum

Scenario: Comment on Post
	Given a runtime fabric for agent ForumAgent
	And I publish the comand PublishPost
	When the command is complete
	And I publish the comand CommentOnPost
	When the command is complete
	Then the query CommentQueries returns the Comment

