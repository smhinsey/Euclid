@SdkSpecs @AgentPanel
Feature: Publish input models as commands
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

Scenario: Publish an input model via the AgentPanel
	
	Given the agent SDKTests.FakeAgent

	# Given the TestComposite running on http://localhost:4997 
	# the above URL should correspond with Euclid.Sdk.TestComposite
	
	When I publish the command TestCommand:
	| Number |
	| 7      |
	And the command is complete

	Then retrieve a List of TestReadModel by running FindByNumber on TestQuery with:
	| Number |
	| 7      |

	# add Composite controller include method to validate composite configuration among other things
	# add InputModel to FakeComposite
	# Watin/HtmlUnit/XBrowser/QUnit tests to test the AgentPanel
