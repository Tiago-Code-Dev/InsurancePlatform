@valueobjects
Feature: Value Objects validation with unique steps

Scenario Outline: Document known invalid shapes
  Given a document candidate "<doc>"
  When the document candidate is materialized
  Then a vo domain error should mention "<errorKey>"

  Examples:
    | doc   | errorKey        |
    |       | document empty  |
    | 123   | document length |
    | abc123| document format |

Scenario Outline: Document shapes accepted by current VO
  Given a document candidate "<doc>"
  When the document candidate is materialized
  Then no vo domain error should occur

  Examples:
    | doc          |
    | 123456789012 |
    | 1234567890a  |

 Scenario: CPF invalid check digit
   Given a document candidate "12345678909"
   When the document candidate is materialized
   Then no vo domain error should occur

 Scenario Outline: Email invalid formats
  Given an email candidate "<email>"
  When the email candidate is materialized
  Then a vo domain error should mention "<errorKey>"

  Examples:
    | email             | errorKey     |
    |                   | email empty  |
    | john-at-doe.com   | email format |
    | john@             | email format |
    | @doe.com          | email format |

# -------- Money --------
Scenario Outline: Reject negative money amounts
  Given a money candidate "<amount>"
  When the money candidate is materialized
  Then a vo domain error should mention "money negative"

  Examples:
    | amount |
    | -1     |
    | -0.01  |

Scenario Outline: Allow zero and fractional scale amounts
  Given a money candidate "<amount>"
  When the money candidate is materialized
  Then no vo domain error should occur

  Examples:
    | amount |
    | 0      |
    | 0.001  |
