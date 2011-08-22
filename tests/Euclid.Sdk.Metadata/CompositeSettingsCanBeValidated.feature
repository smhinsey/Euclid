﻿Feature: Composite settings can be validated
	In order to fail fast and identify configuration errors early
	As a composite developer
	I want to be able to validate configuration and get a description of any errors that occur

@configuration @composite
Scenario: Validating default CompostiteAppSetting fails
	Given A new CompositeAppSetting object
	When I call validate a NullSettingException is thrown
    And NullSettingException.SettingName is equal to 'OutputChannel'
    And There is 1 reason in the enumerable object returned by CompositeAppSetting.GetInvalidSettingReasons() 
    
Scenario: Setting an OutputChannel on the CompositeAppSetting 
    Given A new CompositeAppSetting object
    When I apply an InMemoryMessageChannel to the OutputChannel property
    And I call validate no exceptions are thrown
    And CompositeAppSetting.GetInvalidSettingReasons() returns 0 length enumerable object
