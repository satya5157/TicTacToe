# AI Workflow Notes

## Requirement to Specification Mapping
- Converted the problem statement into backend-first architecture.
- Mapped each functional requirement to a service rule and endpoint behavior.
- Chose Clarification 2 Option A and enforced it in undo behavior.

## Requirement-to-implementation mapping highlights:
- Board, turns, win/draw, reset, and validation rules were implemented in backend domain/service logic.
- Move history, undo behavior by mode, and scoreboard consistency were implemented as server-owned state.
- Computer mode logic was implemented with explicit difficulty strategies.
- Frontend was implemented as a thin client that renders backend responses.

## Prompts and AI Assistance Pattern
- Used AI assistance to:
  - scaffold backend and frontend structure
  - generate baseline domain models, services, controllers, and UI
  - draft test cases and README template
- Used manual review to:
  - verify all acceptance criteria coverage
  - fix nullability and template issues
  - validate undo and scoreboard behavior for Option A

## Prompt Summary (Representative)
- "Build a browser-based Tic Tac Toe with Angular frontend and .NET backend, backend as source of truth."
- "Implement required REST endpoints for game lifecycle, moves, undo, reset, and scoreboard reset."
- "Add tests for valid/invalid moves, win/draw detection, undo by mode, scoreboard updates, and post-completion behavior."
- "Refactor AI/computer logic using Strategy and Factory, and undo behavior using Policy pattern."
- "Update README with prerequisites, run instructions, test steps, and API summary for interview review."

Prompt usage pattern:
- Start with requirement extraction and architecture planning.
- Generate baseline code and contracts.
- Iterate with targeted prompts for bug fixes, refactors, and documentation gaps.
- Re-run tests after each substantial change.

## What AI Generated vs What Was Manually Finalized
AI-generated baseline:
- Initial backend and frontend scaffolding.
- Core API/controller/service skeletons.
- Initial UI layout and styling foundation.
- Initial test and README drafts.

Manually finalized/adjusted:
- Selected .NET 9 and Angular 18 as deliberate project-version choices.
- Clarification 2 Option A behavior details (undo disabled after completion).
- Scoreboard consistency rules and one-time completion counting checks.
- Pattern-oriented refactor (Strategy/Factory/Policy) and wiring.
- Final acceptance-aligned docs and run instructions.

## What Was Carefully Reviewed
- Move validation constraints
- Win/draw detection and winning cells
- Undo behavior by game mode
- Scoreboard updates only once per completed game
- Computer move priority order
- API responses containing full render state

Review depth:
- Cross-checked backend behavior against each functional requirement line item.
- Verified frontend actions call backend endpoints and render returned state.
- Verified mode switching and difficulty handling do not reset scoreboard unexpectedly.

## Assumptions and Trade-offs
- Prioritized correctness and clarity over advanced layering complexity.
- Used in-memory storage for assignment speed and simplicity.
- Implemented minimal frontend tests; emphasized backend unit tests for rule correctness.

Additional trade-off notes:
- Version selection (.NET 9 and Angular 18) was my engineering choice based on local toolchain compatibility and modern framework support.
- Chose in-memory state over SQLite to reduce setup complexity for reviewers.
- Focused heavier testing on backend game-state transitions where most risk exists.
- Kept frontend state minimal to avoid rule duplication and drift from backend truth.

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

## Validation Evidence
- Backend test suite executed and passing after functional and architectural changes.
- Frontend build and unit tests executed and passing after UI/contract updates.
- Lint/diagnostic issues addressed during implementation iterations.
