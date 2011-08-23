@SdkSpec @AgentPanel
Feature: Publish input models as commands
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

Scenario: Publish an input model via the AgentPanel
	Given an agent TestAgent
	And a configured mvc composite running on http://localhost:666
	When I fill out the input model TestInputModel
	Then The command TestCommand should be marked complete
    And The query TestQuery returns data


    # add Composite controller include method to validate composite configuration among other things
    # add InputModel to FakeComposite
    # Watin/HtmlUnit/XBrowser/QUnit tests to test the AgentPanel
    
