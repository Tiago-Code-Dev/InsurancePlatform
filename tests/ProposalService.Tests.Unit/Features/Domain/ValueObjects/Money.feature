Feature: Money
  Validate creation rules for the Money value object

  Scenario: Create a valid positive money value
    Given I have a money amount 100
    When I create the money
    Then the money should be created successfully

  Scenario: Create a money value equal to zero
    Given I have a money amount 0
    When I create the money
    Then the money should be created successfully

  Scenario: Create a negative money value
    Given I have a money amount -50
    When I create the money
    Then a domain exception should be thrown