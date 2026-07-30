using TicTacToe.Api.Models;
using TicTacToe.Api.Services.GameRules;

namespace TicTacToe.Api.Services.ComputerStrategies;

public sealed class MediumComputerMoveStrategy : IComputerMoveStrategy
{
    private static readonly CellPosition[] PreferredCorners =
    {
        new(0, 0), new(0, 2), new(2, 0), new(2, 2)
    };

    public ComputerDifficulty Difficulty => ComputerDifficulty.Medium;

    public CellPosition ChooseMove(GameSession game)
    {
        var winningMove = BoardAnalyzer.FindCriticalMove(game.Board, Player.O);
        if (winningMove is not null)
        {
            return winningMove;
        }

        var blockMove = BoardAnalyzer.FindCriticalMove(game.Board, Player.X);
        if (blockMove is not null)
        {
            return blockMove;
        }

        if (!game.Board[1, 1].HasValue)
        {
            return new CellPosition(1, 1);
        }

        var corner = PreferredCorners.FirstOrDefault(c => !game.Board[c.Row, c.Column].HasValue);
        if (corner is not null)
        {
            return corner;
        }

        var any = BoardAnalyzer.GetAvailableCells(game.Board).FirstOrDefault();
        if (any is not null)
        {
            return any;
        }

        throw new InvalidOperationException("No valid computer move available.");
    }
}
