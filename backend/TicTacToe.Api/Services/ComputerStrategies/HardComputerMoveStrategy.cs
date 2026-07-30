using TicTacToe.Api.Models;
using TicTacToe.Api.Services.GameRules;

namespace TicTacToe.Api.Services.ComputerStrategies;

public sealed class HardComputerMoveStrategy : IComputerMoveStrategy
{
    public ComputerDifficulty Difficulty => ComputerDifficulty.Hard;

    public CellPosition ChooseMove(GameSession game)
    {
        var board = game.Board;
        var available = BoardAnalyzer.GetAvailableCells(board);
        if (available.Count == 0)
        {
            throw new InvalidOperationException("No valid computer move available.");
        }

        var bestScore = int.MinValue;
        var bestMove = available[0];

        foreach (var cell in available)
        {
            board[cell.Row, cell.Column] = Player.O;
            var score = Minimax(board, maximizing: false, depth: 0);
            board[cell.Row, cell.Column] = null;

            if (score > bestScore)
            {
                bestScore = score;
                bestMove = cell;
            }
        }

        return bestMove;
    }

    private static int Minimax(Player?[,] board, bool maximizing, int depth)
    {
        if (BoardAnalyzer.TryGetWinner(board, out var winner, out _))
        {
            if (winner == Player.O)
            {
                return 10 - depth;
            }

            return depth - 10;
        }

        if (BoardAnalyzer.IsDraw(board))
        {
            return 0;
        }

        if (maximizing)
        {
            var best = int.MinValue;
            foreach (var cell in BoardAnalyzer.GetAvailableCells(board))
            {
                board[cell.Row, cell.Column] = Player.O;
                var score = Minimax(board, maximizing: false, depth: depth + 1);
                board[cell.Row, cell.Column] = null;
                best = Math.Max(best, score);
            }

            return best;
        }

        var worst = int.MaxValue;
        foreach (var cell in BoardAnalyzer.GetAvailableCells(board))
        {
            board[cell.Row, cell.Column] = Player.X;
            var score = Minimax(board, maximizing: true, depth: depth + 1);
            board[cell.Row, cell.Column] = null;
            worst = Math.Min(worst, score);
        }

        return worst;
    }
}
