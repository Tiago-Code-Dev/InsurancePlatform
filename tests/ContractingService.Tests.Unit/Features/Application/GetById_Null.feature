@app-contract
Feature: GetById null path (all operations)

Scenario: Activate non-existing contract should fail
  Given there is no current contract in repository
  When the application activates the current contract
  Then the app response should be an internal server error
  And the repository should not have persisted any change

Scenario: Cancel non-existing contract should fail
  Given there is no current contract in repository
  When the application cancels the current contract
  Then the app response should be an internal server error
  And the repository should not have persisted any change

Scenario: Terminate non-existing contract should fail
  Given there is no current contract in repository
  When the application terminates the current contract
  Then the app response should be an internal server error
  And the repository should not have persisted any change
