@SdkSpecs @MetadataService @CompositeMetadata
Feature: Composite provides configuration metadata
	In order to satisfy requests for metadata
	As a composite
	I need to provide metadata in arbitrary formats

Scenario Outline: Composite can provide metadata about it's configuration 
	Given a composite that <is-or-isn't> configured
	When I call IsValid
	Then the result should be <true-or-false>
	And the call to GetConfigurationErrors returns an enumerable list that contains <item-count> items

Examples: 
	| is-or-isn't | true-or-false | item-count |
	| is          | true          | 0          |
	| isn't       | false         | 1          |


Scenario Outline: Composite provides formatted metadata
	Given a composite that <is-or-isn't> configured
	And it contains the TestAgent
	And it contains the TestInputModel
	When metadata is requested as <format-name>
	Then it can be represented as <content-type>
	And has been independently validated

 Examples:
	| is-or-isn't | format-name | content-type     |
	| is          | xml         | text/xml         |
	| is          | json        | application/json |
	| isn't       | xml         | text/xml         |
	| isn't       | json        | application/json |
