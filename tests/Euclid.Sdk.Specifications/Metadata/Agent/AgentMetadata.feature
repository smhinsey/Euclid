@SdkSpecs @MetadataService @AgentMetadata
Feature: Euclid agents provide metadata
	In order to satisfy requests for metadata
	As an agent
	I need to provide metadata in arbitrary formats

Scenario Outline: : Agents have 2 types of metadata representations
    Given an agent
    When the <representation-type> is requested
    When metadata is requested as <format-name>
    Then it can be represented as <content-type>
    And has been independently validated

Examples: 
    | representation-type | format-name | content-type     |
    | basic               | xml         | text/xml         |
    | basic               | json        | application/json |
    | full                | xml         | text/xml         |
    | full                | json        | application/json |

Scenario Outline: Agent parts provide formatted metadata
	Given the part <agent-part>
	When metadata is requested as <format-name>
    Then it can be represented as <content-type>
    And has been independently validated

Examples:
    | agent-part | format-name | content-type     |
    | command    | xml         | text/xml         |
    | command    | json        | application/json |
    | query      | xml         | text/xml         |
    | query      | json        | application/json |
    | readmodel  | xml         | text/xml         |
    | readmodel  | json        | application/json |

# todo: future content types
#    | xhtml       | text/html        |

Scenario Outline: Agent part collections provide formatted metadata
	Given a part collection <descriptive-name>
	When metadata is requested as <format-name>
    Then it can be represented as <content-type>
    And has been independently validated

Examples:
    | descriptive-name | format-name | content-type     |
    | commands         | xml         | text/xml         |
    | commands         | json        | application/json |
    | queries          | xml         | text/xml         |
    | queries          | json        | application/json |
    | readmodels       | xml         | text/xml         |
    | readmodels       | json        | application/json |

Scenario Outline: Collections of agent metadata provide 2 types of formatted metadata
    Given an agent collection
    When the <representation-type> is requested from the collection
    When metadata is requested as <format-name>
    Then it can be represented as <content-type>
    And has been independently validated

Examples: 
    | representation-type | format-name | content-type     |
    | basic               | xml         | text/xml         |
    | basic               | json        | application/json |
    | full                | xml         | text/xml         |
    | full                | json        | application/json |