using TicTacToe.Api.Contracts;
using TicTacToe.Api.Models;

namespace TicTacToe.Api.Services;

public interface IGameService
{
    GameStateResponse CreateGame(GameMode mode, ComputerDifficulty difficulty = ComputerDifficulty.Medium);

    GameStateResponse GetGame(Guid gameId);

    GameStateResponse MakeMove(Guid gameId, Player player, int row, int column);

    GameStateResponse Undo(Guid gameId);

    GameStateResponse ResetGame(Guid gameId);

    ScoreboardResponse GetScoreboard();

    ScoreboardResponse ResetScoreboard();
}
