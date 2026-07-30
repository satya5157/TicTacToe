# Tic Tac Toe - Angular + .NET

This repository contains a complete Tic Tac Toe solution with:
- Angular frontend in `frontend/tictactoe-ui`
- .NET Web API backend in `backend/TicTacToe.Api`

The backend is the source of truth for game rules, validation, game status, move history, and scoreboard.

## Tech Stack
- Frontend: Angular 18 + TypeScript
- Backend: ASP.NET Core Web API (.NET 9)
- API Style: REST
- Storage: In-memory
- Tests: xUnit for backend, Jasmine/Karma for frontend component basics

## Implemented Features
- 3x3 board with lock on occupied cells
- Two Player mode
- Play Against Computer mode
- Computer difficulty levels (Easy, Medium, Hard)
- Turn handling and invalid move validation
- Win detection (row/column/diagonal)
- Draw detection
- Winning cell highlighting
- Move history table
- Undo Last Move with mode-specific behavior
- Session scoreboard (X wins, O wins, draws)
- Reset Game (scoreboard unchanged)
- Reset Scoreboard

## Clarification Choice
- Clarification 2 (Scoreboard and Undo): **Option A** implemented
  - Undo is disabled once game status is Won or Draw
  - Scoreboard result remains final for completed games

## Folder Structure
- `backend/`
  - `TicTacToe.sln`
  - `TicTacToe.Api/` (Web API)
  - `TicTacToe.Api.Tests/` (unit tests)
- `frontend/`
  - `tictactoe-ui/` (Angular source)

This structure is ready for separate frontend/backend source uploads.

## Backend Run Instructions
1. Open terminal in `backend`
2. Run:
   - `dotnet restore`
   - `dotnet run --project TicTacToe.Api`
3. API base URL (default): `http://localhost:5249`

## Frontend Run Instructions
1. Install Node.js 20+ and npm if not installed
2. Open terminal in `frontend/tictactoe-ui`
3. Run:
   - `npm install`
   - `npm start`
4. App URL: `http://localhost:4200`

## API Endpoint Summary
### Game APIs
- `POST /api/games`
  - Create new game session
  - Body: `{ "mode": "TwoPlayer" | "VsComputer", "difficulty": "Easy" | "Medium" | "Hard" }`
- `GET /api/games/{id}`
  - Fetch current game state
- `POST /api/games/{id}/moves`
  - Submit move
  - Body: `{ "player": "X" | "O", "row": 0-2, "column": 0-2 }`
- `POST /api/games/{id}/undo`
  - Undo last move or move pair based on mode
- `POST /api/games/{id}/reset`
  - Reset current game board/history/status

### Scoreboard APIs
- `GET /api/scoreboard`
- `POST /api/scoreboard/reset`

## Game State Response
All game mutation endpoints return full state required by UI:
- `gameId`
- `mode`
- `board`
- `currentPlayer`
- `status` (`InProgress`, `Won`, `Draw`)
- `winner` (if any)
- `winningCells` (if any)
- `moveHistory`
- `scoreboard`

## Test Instructions
### Backend tests
From `backend`:
- `dotnet test TicTacToe.sln`

Covered backend unit scenarios:
- valid move
- invalid move
- turn switching
- row win
- column win
- diagonal win
- draw
- reset game
- undo in two-player mode
- undo in computer mode
- scoreboard update behavior
- computer move selection
- move after completion

### Frontend tests
From `frontend/tictactoe-ui`:
- `npm test`

## AI Tools and Prompt Summary
See `AI_WORKFLOW.md` for:
- how requirements were translated into implementation
- what was generated and what was manually adjusted
- trade-offs and review checks

## Design Decisions
- Backend owns canonical state to avoid rule divergence
- In-memory state used for simplicity and speed
- Single service (`GameService`) encapsulates rule engine and transitions
- Full game state returned after every mutation to keep frontend simple

## Architectural Patterns Used
- Service Layer: `GameService` orchestrates state transitions and use-case flows
- Strategy: AI behavior encapsulated by `IComputerMoveStrategy` implementations
- Factory: `IComputerMoveStrategyFactory` resolves strategy by difficulty
- Policy: Undo behavior encapsulated by `IUndoPolicy` per game mode
- Adapter/Mapping: Domain model to API DTO mapping via mapping extensions
- Singleton lifetime: API-level state service registered as singleton for session memory

## Assumptions
- Session-level scoreboard resets only via explicit reset endpoint
- One backend process maintains in-memory game sessions for demo scope
- In computer mode, human is always X and computer is O

## Known Limitations
- In-memory storage is not persistent across backend restarts
- No authentication or multi-user identity separation
- Frontend test coverage is minimal compared to backend logic tests

## Future Improvements
- Add persistent storage (SQLite)
- Add integration tests for API controllers
- Add richer frontend tests and e2e coverage
- Add adaptive difficulty mode (dynamic difficulty tuning based on player performance)
