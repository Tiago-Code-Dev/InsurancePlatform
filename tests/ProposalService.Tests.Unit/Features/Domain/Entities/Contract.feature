Feature: Contract
  Validate creation rules for the Contract entity

  Scenario: Create a valid contract
    Given I have a valid contract type
    And I have a premium amount 100
    And I have a start date today
    And I have an end date one year from today
    When I create the contract
    Then the contract should be created successfully

  Scenario: Create a contract with negative premium
    Given I have a valid contract type
    And I have a premium amount -50
    And I have a start date today
    And I have an end date one year from today
    When I create the contract
    Then a domain exception should be thrown

  Scenario: Create a contract with end date before start date
    Given I have a valid contract type
    And I have a premium amount 100
    And I have a start date today
    And I have an end date yesterday
    When I create the contract
    Then a domain exception should be thrown