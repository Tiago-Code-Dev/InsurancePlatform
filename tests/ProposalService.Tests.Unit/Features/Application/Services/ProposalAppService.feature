Feature: Proposal Application Service
  Validate proposal creation and lifecycle management via the application service

  Scenario: Create a valid proposal
    Given I have a valid customer DTO
    And I have a valid contract DTO
    When I call the service to create the proposal
    Then the response should be created successfully
    And the repository AddAsync should be called
    And the event publisher PublishAsync should be called

  Scenario: Create a proposal with invalid document
    Given I have a customer DTO with document "12345"
    And I have a valid contract DTO
    When I call the service to create the proposal
    Then the response should fail with message "An internal error has occurred. Please try again later."

  Scenario: Create a proposal with invalid email
    Given I have a customer DTO with email "invalid.com"
    And I have a valid contract DTO
    When I call the service to create the proposal
    Then the response should fail with message "An internal error has occurred. Please try again later."

  Scenario: Create a proposal with negative premium
    Given I have a valid customer DTO
    And I have a contract DTO with premium -100
    When I call the service to create the proposal
    Then the response should fail with message "An internal error has occurred. Please try again later."

  Scenario: Get an existing proposal by ID
    Given a proposal exists in the repository
    When I call the service to get the proposal by ID
    Then the response should return the proposal successfully

  Scenario: Get a proposal by non-existing ID
    Given no proposal exists with the requested ID
    When I call the service to get the proposal by ID
    Then the response should fail with message "Proposta não encontrada."

  Scenario: Approve an existing proposal
    Given a proposal exists in the repository
    When I call the service to approve the proposal
    Then the proposal should be approved
    And the repository UpdateAsync should be called

  Scenario: Approve a non-existing proposal
    Given no proposal exists with the requested ID
    When I call the service to approve the proposal
    Then the response should fail with message "Proposta não encontrada."

  Scenario: Reject an existing proposal
    Given a proposal exists in the repository
    When I call the service to reject the proposal
    Then the proposal should be rejected
    And the repository UpdateAsync should be called

  Scenario: Reject a non-existing proposal
    Given no proposal exists with the requested ID
    When I call the service to reject the proposal
    Then the response should fail with message "Proposta não encontrada."
