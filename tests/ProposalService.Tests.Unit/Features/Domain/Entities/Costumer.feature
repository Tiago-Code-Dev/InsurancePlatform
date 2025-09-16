Feature: Customer
  Validate creation rules for the Customer entity

  Scenario: Create a valid customer
    Given I have a customer name "John Doe"
    And I have a valid document number "12345678901"
    And I have a valid email "john.doe@email.com"
    When I create the customer
    Then the customer should be created successfully

  Scenario: Create a customer without a name
    Given I have a customer name ""
    And I have a valid document number "12345678901"
    And I have a valid email "john.doe@email.com"
    When I create the customer
    Then a domain exception should be thrown

  Scenario: Create a customer with an invalid document
    Given I have a customer name "Jane Doe"
    And I have a valid document number "12345"
    And I have a valid email "jane.doe@email.com"
    When I create the customer
    Then a domain exception should be thrown

  Scenario: Create a customer with an invalid email
    Given I have a customer name "Jane Doe"
    And I have a valid document number "12345678901"
    And I have a valid email "invalidemail.com"
    When I create the customer
    Then a domain exception should be thrown