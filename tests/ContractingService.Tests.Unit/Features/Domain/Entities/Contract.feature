Feature: Contract
  Validate contract creation, coverage rules, and activation

  Scenario: Create a valid contract
    Given I have a valid insured
    When I create the contract
    Then the contract should be created successfully
    And the contract status should be "Draft"

  Scenario: Create a contract without insured
    Given I have no insured
    When I create the contract
    Then an operation exception should be thrown with message "insured"

  Scenario: Add a coverage to a draft contract
    Given I have a valid contract
    And I have a valid coverage
    When I add the coverage to the contract
    Then the coverage should be added successfully

  Scenario: Add a coverage to an active contract
    Given I have a valid contract
    And I have a valid coverage
    And I activate the contract with the coverage
    When I try to add another coverage
    Then a domain exception should be thrown with message "Coverages can only be added while contract is in Draft status."

  Scenario: Activate a contract without coverages
    Given I have a valid contract
    When I try to activate the contract
    Then a domain exception should be thrown with message "A contract must have at least one coverage to be activated."

  Scenario: Activate a contract with coverages
    Given I have a valid contract
    And I have a valid coverage
    When I activate the contract with the coverage
    Then the contract status should be "Active"
    And the contract should have an activation date

  Scenario: Cancel an active contract
    Given I have a valid contract
    And I have a valid coverage
    And I activate the contract with the coverage
    When I cancel the contract
    Then the contract should be canceled

  Scenario: Try to cancel a non-active contract
    Given I have a valid contract
    When I cancel the contract
    Then a domain exception should be thrown containing message "only active"

  Scenario: Terminate an active contract
   Given I have a valid contract
   And I have a valid coverage
   And I activate the contract with the coverage
   When I terminate the contract
   Then the contract should be terminated

 Scenario: Try to terminate a non-active contract
   Given I have a valid contract
   When I terminate the contract
   Then the contract should be terminated 

 Scenario: Activate a contract with coverage
  Given I have a valid contract
  And I have a valid coverage
  And I activate the contract with the coverage
  Then the contract status should be "Active"
  And the contract should have an activation date

Scenario: Add a coverage only in Draft
  Given I have a valid contract
  And I have a valid coverage
  When I add the coverage to the contract
  Then the coverage should be added successfully

Scenario: Try to add coverage when contract is Active
  Given I have a valid contract
  And I have a valid coverage
  And I activate the contract with the coverage
  When I try to add another coverage
  Then a domain exception should be thrown with message "in Draft status"

Scenario: Validate contract without insured
  Given I have a valid contract
  And I have no insured
  When I create the contract
  Then an operation exception should be thrown with message "insured"

Scenario: Validate contract without coverages
  Given I have a valid contract
  When I try to activate the contract
  Then a domain exception should be thrown with message "A contract must have at least one coverage to be activated."

 Scenario: Activates a contract with coverages
  Given I have a valid contract
  And I have a valid coverage
  When I activate the contract with the coverage
  Then the contract status should be "Active"
  And the contract should have an activation date
  And no exception should be thrown