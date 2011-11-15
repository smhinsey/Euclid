@SdkSpecs
Feature: Input models are converted to commands
	In order to decouple user input from commands
	As a developer
	I want the composite app to be able to convert an object of type IInputModel to an object of type ICommand

@SdkSpecs
Scenario: Register an input model and command
Given a registered inputmodel and command
When GetCommandMetadataForInputModel is called
Then the command is returned

@SdkSpecs
Scenario: Register a single input model for multiple commands
Given a registered inputmodel and command
When the same inputmodel is registered for a new command
Then a InputModelAlreadyRegisteredException exception is thrown

@SdkSpecs
Scenario: Register multiple input models for a command
Given a registered inputmodel and command
When a new inputmodel is registered for an existing command
Then a CommandAlreadyMappedException exception is thrown

# Scenario: Post input model to controller
# Given a registered inputmodel and command
# When an inputmodel is posted to the compositeinspector
# Then the appropriate command is published

# Scenario: Post complex input model to controller
# Given a registered inputmodel and command with custom mapping function
# When an inputmodel is posted to the compositeinspector
# Then the appropriate command is published

