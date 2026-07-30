# AI Workflow Notes

## Requirement to Specification Mapping
- Converted the problem statement into backend-first architecture.
- Mapped each functional requirement to a service rule and endpoint behavior.
- Chose Clarification 2 Option A and enforced it in undo behavior.

## Prompts and AI Assistance Pattern
- Used AI assistance to:
  - scaffold backend and frontend structure
  - generate baseline domain models, services, controllers, and UI
  - draft test cases and README template
- Used manual review to:
  - verify all acceptance criteria coverage
  - fix nullability and template issues
  - validate undo and scoreboard behavior for Option A

## What Was Carefully Reviewed
- Move validation constraints
- Win/draw detection and winning cells
- Undo behavior by game mode
- Scoreboard updates only once per completed game
- Computer move priority order
- API responses containing full render state

## Assumptions and Trade-offs
- Prioritized correctness and clarity over advanced layering complexity.
- Used in-memory storage for assignment speed and simplicity.
- Implemented minimal frontend tests; emphasized backend unit tests for rule correctness.

## Architecture Enhancements
- Refactored computer move logic to Strategy + Factory pattern.
- Refactored undo behavior by game mode using Policy pattern.
- Kept singleton service lifetime for fast in-memory session operations.
- Added configurable computer difficulty: Easy, Medium, Hard.

## Discussed but Not Implemented
- Adaptive difficulty mode was discussed and recorded as a future extension in README.

## Manual Changes After Generation
- Corrected AI move corner-selection nullability warning.
- Corrected Angular template structure for nested board loops.
- Aligned README with submission checklist and separate source-folder expectation.
