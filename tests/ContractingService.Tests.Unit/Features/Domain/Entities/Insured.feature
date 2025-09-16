@insured
Feature: Insured validation with unique steps

  Scenario Outline: Reject invalid insured data
    Given an insured blueprint with name "<name>" document "<doc>" email "<email>"
    When the insured blueprint is materialized
    Then a domain error should mention "<errorKey>"

    Examples:
      | name       | doc          | email            | errorKey      |
      |            | 12345678901  | john@doe.com     | insured name  |
      | John Doe   |              | john@doe.com     | document      |
      | John Doe   | 12345678901  | john-at-doe.com  | email         |
