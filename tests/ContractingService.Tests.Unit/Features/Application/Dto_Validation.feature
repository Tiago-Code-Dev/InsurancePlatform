@app-contract
Feature: Application DTO validation

Scenario Outline: Create contract with invalid insured data
  When the application tries to create a contract with name "<name>" doc "<doc>" email "<email>" and 0 coverages
  Then the app response should be an internal server error
  And the repository should not have persisted any change

  Examples:
    | name     | doc          | email           |
    |          | 12345678901  | john@doe.com    |
    | John Doe |              | john@doe.com    |
    | John Doe | 12345678901  | john-at-doe.com |

Scenario: Create contract with invalid coverage DTO (type unknown)
  When the application tries to create a contract with 1 coverage named "X" type "Unknown" premium 100
  Then the app response should be an internal server error
  And the repository should not have persisted any change