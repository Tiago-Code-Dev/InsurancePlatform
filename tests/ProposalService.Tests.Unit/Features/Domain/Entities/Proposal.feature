Feature: Proposal
  Validate creation rules for the Proposal entity

Scenario: Create a valid proposal
  Given I have a valid customer
  And I have a valid contract
  When I create the proposal
  Then the proposal should be created successfully
  And the proposal should have initial status "Pending"

  Scenario: Create a proposal without a customer
    Given I have no customer
    And I have a valid contract
    When I create the proposal
    Then a domain exception should be thrown

  Scenario: Create a proposal without a contract
    Given I have a valid customer
    And I have no contract
    When I create the proposal
    Then a domain exception should be thrown
