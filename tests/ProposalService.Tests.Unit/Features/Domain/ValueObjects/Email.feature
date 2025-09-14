Feature: Email
  Validate creation rules for the Email value object

  Scenario: Create a valid email
    Given I have an email address "user@example.com"
    When I create the email
    Then the email should be created successfully

  Scenario: Create another valid email
    Given I have an email address "john.doe@domain.org"
    When I create the email
    Then the email should be created successfully

  Scenario: Create an empty email
    Given I have an email address ""
    When I create the email
    Then a domain exception should be thrown

  Scenario: Create an invalid email without at sign
    Given I have an email address "invalidemail.com"
    When I create the email
    Then a domain exception should be thrown

  Scenario: Create an invalid email without domain
    Given I have an email address "abc@"
    When I create the email
    Then a domain exception should be thrown