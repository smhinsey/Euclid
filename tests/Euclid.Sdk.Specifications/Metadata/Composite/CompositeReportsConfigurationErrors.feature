@SdkSpecs @MetadataService @CompositeMetadata @Configuration
Feature: Composite reports configuration errors
	In order to learn if my composite app is configured correctly
	As a composite application developer
	I want to be to query a composite for it's configuration errors

Scenario: Composite is incorrectly configured
	Given A composite that hasn't been configured
	When I call HasConfigurationErrors
	Then The result should be true
	When I call GetConfigurationErrors
	Then I receive an enumerable list of error descriptions with 1 or more items in it
    