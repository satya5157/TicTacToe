# Backend - TicTacToe.Api

## Projects
- `TicTacToe.Api`: ASP.NET Core Web API
- `TicTacToe.Api.Tests`: xUnit unit tests

## Run
From `backend`:
- `dotnet restore`
- `dotnet run --project TicTacToe.Api`

Default URL: `http://localhost:5249`

## Test
From `backend`:
- `dotnet test TicTacToe.sln`

## Notes
- Backend owns game/session state and scoreboard.
- Undo after completion is disabled (Clarification 2 Option A).
- Computer mode supports difficulty levels: Easy, Medium, Hard.
- Patterns used: Service Layer, Strategy + Factory for AI, Policy for Undo, Singleton lifetime.
- Planned extension (discussion): Adaptive difficulty mode.
