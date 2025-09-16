@app-contract
Feature: Terminate Contract via Application

  Scenario: Terminate a draft contract (allowed by domain)
    Given an existing draft contract without coverage
    When the application terminates the current contract
    Then the app response should be successful
    And the repository should have persisted the contract with status "Terminated"

  Scenario: Reject terminating twice
    Given an already terminated contract
    When the application terminates the current contract
    Then the app response should be an internal server error
    And the repository should not have persisted any change