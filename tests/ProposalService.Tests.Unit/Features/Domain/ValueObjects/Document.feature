Feature: Document
  Validate creation rules for the Document value object

  Scenario: Create a valid document with 11 characters
    Given I have a document number "12345678901"
    When I create the document
    Then the document should be created successfully

  Scenario: Create a valid document with 14 characters
    Given I have a document number "12345678901234"
    When I create the document
    Then the document should be created successfully

  Scenario: Create a document with less than 11 characters
    Given I have a document number "12345"
    When I create the document
    Then a domain exception should be thrown

  Scenario: Create a document with more than 14 characters
    Given I have a document number "12345678901234567"
    When I create the document
    Then a domain exception should be thrown

  Scenario: Create a document with an empty number
    Given I have a document number ""
    When I create the document
    Then a domain exception should be thrown