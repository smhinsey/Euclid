@SdkSpecs @MetadataService @CompositeMetadata
Feature: Euclid CompositeApplications provide metadata
	In order to satisfy requests for metadata
	As a composite
	I need to provide metadata in arbitrary formats

Scenario: Add two numbers
	Given I have entered 50 into the calculator
	And I have entered 70 into the calculator
	When I press add
	Then the result should be 120 on the screen
