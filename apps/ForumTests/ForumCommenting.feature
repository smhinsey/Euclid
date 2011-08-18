﻿@ForumAgentSpecs
Feature: Forum Commenting
	In order to interact with a Forum
	As a Forum User
	I want to create Posts in that Forum

Scenario: Comment on Post
	Given the agent ForumAgent

	And I publish the command PublishPost
	When the command is complete

	And I publish the command CommentOnPost
	When the command is complete

	Then the query CommentQueries returns the Comment
