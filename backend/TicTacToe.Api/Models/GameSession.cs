namespace TicTacToe.Api.Models;

public sealed class GameSession
{
    public Guid Id { get; init; } = Guid.NewGuid();

    public GameMode Mode { get; init; }

    public ComputerDifficulty Difficulty { get; init; } = ComputerDifficulty.Medium;

    public Player?[,] Board { get; } = new Player?[3, 3];

    public Player CurrentPlayer { get; set; } = Player.X;

    public GameStatus Status { get; set; } = GameStatus.InProgress;

    public Player? Winner { get; set; }

    public List<CellPosition> WinningCells { get; } = new();

    public List<MoveRecord> MoveHistory { get; } = new();

    public bool IsResultCounted { get; set; }
}
