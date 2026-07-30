using TicTacToe.Api.Models;
using TicTacToe.Api.Services.GameRules;

namespace TicTacToe.Api.Services.ComputerStrategies;

public sealed class EasyComputerMoveStrategy : IComputerMoveStrategy
{
    public ComputerDifficulty Difficulty => ComputerDifficulty.Easy;

    public CellPosition ChooseMove(GameSession game)
    {
        var available = BoardAnalyzer.GetAvailableCells(game.Board);
        if (available.Count == 0)
        {
            throw new InvalidOperationException("No valid computer move available.");
        }

        return available[Random.Shared.Next(available.Count)];
    }
}
