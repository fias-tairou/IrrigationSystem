Feature: Irrigation System
  Test that the irrigation system enables under dry conditions.

  Scenario: Enable irrigation when soil is dry and no rain
    Given the soil moisture is below the threshold
    And the rainfall is below the threshold
    When the irrigation scheduler runs
    Then the irrigation system should be enabled