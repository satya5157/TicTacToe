using TicTacToe.Api.Models;

namespace TicTacToe.Api.Contracts;

public sealed class CreateGameRequest
{
    public GameMode Mode { get; set; } = GameMode.TwoPlayer;

    public ComputerDifficulty Difficulty { get; set; } = ComputerDifficulty.Medium;
}
