Feature: File

  # --- Positive сases ---
Scenario: TC-1 | Create file with valid name and content
   Given a filename "doc.txt" and content "hello"
    When I create a File object
    Then the filename should be "doc.txt"
    And the size should equal half the content length (2.5)
    And the extension should be "txt"


Scenario: TC-2 | Get filename from file
    Given a File with filename "doc.txt"
    When I call GetFileName
    Then I should get "doc.txt"


Scenario: TC-3 | Get size from file with even length content
    Given a File with content "abcd" (length 4)
    When I call GetSize
    Then I should get 2


# --- Negative сases ---

Scenario: TC-4 | File extension parsing should work correctly
    Given a filename "archive.tar.gz"
    When I create a File object
    Then the extension should be "gz"


Scenario: TC-5 | File size should be calculated with decimals
    Given a File with content "abc" (length 3)
    When I call GetSize
    Then I should get 1.5
