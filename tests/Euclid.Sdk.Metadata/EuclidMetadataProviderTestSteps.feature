@Feature
Feature: Euclid agents provide metadata about their parts
	In order to satisfy requests for metadata
	As an agent
	I need to provide metadata in arbitrary formats

Scenario Outline: Supported Agent metadata formats
	Given an agent
	When metadata is requested as <format-name>
    Then can be represented as <content-type>
    And has been independently validated

Examples:
    | format-name | content-type     |
    | xml         | text/xml         |
    | json        | application/json |

# todo: future content types
#    | xhtml       | text/html        |