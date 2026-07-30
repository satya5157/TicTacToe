using TicTacToe.Api.Models;

namespace TicTacToe.Api.Contracts;

public sealed class GameStateResponse
{
    public Guid GameId { get; init; }

    public GameMode Mode { get; init; }

    public ComputerDifficulty Difficulty { get; init; }

    public string?[][] Board { get; init; } = Array.Empty<string?[]>();

    public Player CurrentPlayer { get; init; }

    public GameStatus Status { get; init; }

    public Player? Winner { get; init; }

    public IReadOnlyList<CellPosition> WinningCells { get; init; } = Array.Empty<CellPosition>();

    public IReadOnlyList<MoveResponse> MoveHistory { get; init; } = Array.Empty<MoveResponse>();

    public ScoreboardResponse Scoreboard { get; init; } = new(0, 0, 0);
}
