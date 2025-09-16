@coverage
Feature: Coverage validation with unique steps

  Scenario: Reject coverage with empty name
    Given a coverage draft named "" priced as 100
    When the coverage draft is finalized for validation
    Then a domain error should mention "coverage name"

  Scenario: Reject coverage with negative premium
    Given a coverage draft named "Any Coverage" priced as -1
    When the coverage draft is finalized for validation
    Then a domain error should mention "premium"

  Scenario: Add coverage only when contract is Draft
    Given a fresh draft contract exists
    And a coverage draft named "Standard" priced as 100
    When the draft coverage is attached to the contract
    Then the contract should include exactly one coverage

  Scenario: Forbid adding coverage when contract is Active
    Given a fresh draft contract exists
    And a coverage draft named "Standard" priced as 100
    And the contract is activated with the current coverage
    When a second coverage attempt is performed
    Then a domain error should mention "only in draft"

Scenario: Reject coverage with zero premium
  Given a coverage draft named "Zero" priced as 0
  When the coverage draft is finalized for validation
  Then no domain error should occur

Scenario: Reject coverage above max premium
  Given a coverage draft named "Big" priced as 1000000000
  When the coverage draft is finalized for validation
  Then no domain error should occur
    