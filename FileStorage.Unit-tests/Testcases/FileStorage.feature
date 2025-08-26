Feature: FileStorage

  # --- Positive сases ---

Scenario: TC-1 | Create storage with default size
    Given no parameters
    When I create a FileStorage
    Then available size should be 100


Scenario: TC-2 | Create storage with custom size
    Given a size parameter 200
    When I create a FileStorage
    Then available size should be 200


Scenario: TC-3 | Successfully write a file into storage
    Given a FileStorage with size 100
    And a File of size 10
    When I call Write
    Then the file should be stored
    And available size should decrease by 10


Scenario: TC-4 | Check file existence after writing
    Given a FileStorage with size 100
    And a file "doc.txt" written into it
    When I call IsExists("doc.txt")
    Then it should return true


Scenario: TC-5 | Delete existing file
    Given a FileStorage with size 100
    And a file "doc.txt" written into it
    When I call Delete("doc.txt")
    Then the file should be removed
    And IsExists("doc.txt") should return false


Scenario: TC-6 | Get file by name
    Given a FileStorage with size 100
    And a file "doc.txt" written into it
    When I call GetFile("doc.txt")
    Then it should return that file


# --- Negative Cases ---

Scenario: TC-7 | Custom storage size should not be increased by default
    Given a storage created with size 200
    When I check available size
    Then I should get 200


Scenario: TC-8 | Write file larger than available space
    Given a storage with size 50
    And a file with size 100
    When I try to write the file
    Then it should return false


Scenario: TC-9 | Write file equal to available size
    Given a storage with size 10
    And a file with size 10
    When I try to write the file
    Then it should return true


Scenario: TC-10 | Write file with duplicate name
    Given a storage with size 100
    And a file "doc.txt" already exists
    When I try to write another file "doc.txt"
    Then it should throw FileNameAlreadyExistsException


Scenario: TC-11 | Check existence with substring of filename
    Given a file "document.txt" in storage
    When I call IsExists("doc")
    Then it should return false


Scenario: TC-12 | Get non-existent file
    Given an empty storage
    When I call GetFile("ghost.txt")
    Then it should return null


Scenario: TC-13 | Delete non-existent file
    Given an empty storage
    When I call Delete("ghost.txt")
    Then it should return false