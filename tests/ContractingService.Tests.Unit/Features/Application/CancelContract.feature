@app-contract
Feature: Cancel Contract via Application

  Scenario: Cancel an active contract
    Given an active contract (created via application)
    When the application cancels the current contract
    Then the app response should be successful
    And the repository should have persisted the contract with status "Canceled"

  Scenario: Reject cancel when not active
    Given an existing draft contract without coverage
    When the application cancels the current contract
    Then the app response should be an internal server error
    And the repository should not have persisted any change
