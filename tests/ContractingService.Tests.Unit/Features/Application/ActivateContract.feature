@app-contract
Feature: Activate Contract via Application
  Uses ContractAppService to activate contracts

  Scenario: Activate when contract has at least one coverage
    Given an existing draft contract with one coverage
    When the application activates the current contract
    Then the app response should be successful
    And the repository should have persisted the contract with status "Active"

  Scenario: Reject activation without coverages
    Given an existing draft contract without coverage
    When the application activates the current contract
    Then the app response should be an internal server error
    And the repository should not have persisted any change