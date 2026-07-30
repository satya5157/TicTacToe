using TicTacToe.Api.Models;

namespace TicTacToe.Api.Services.GameRules;

public static class BoardAnalyzer
{
    public static readonly CellPosition[][] WinningLines =
    {
        new[] { new CellPosition(0, 0), new CellPosition(0, 1), new CellPosition(0, 2) },
        new[] { new CellPosition(1, 0), new CellPosition(1, 1), new CellPosition(1, 2) },
        new[] { new CellPosition(2, 0), new CellPosition(2, 1), new CellPosition(2, 2) },
        new[] { new CellPosition(0, 0), new CellPosition(1, 0), new CellPosition(2, 0) },
        new[] { new CellPosition(0, 1), new CellPosition(1, 1), new CellPosition(2, 1) },
        new[] { new CellPosition(0, 2), new CellPosition(1, 2), new CellPosition(2, 2) },
        new[] { new CellPosition(0, 0), new CellPosition(1, 1), new CellPosition(2, 2) },
        new[] { new CellPosition(0, 2), new CellPosition(1, 1), new CellPosition(2, 0) }
    };

    public static IReadOnlyList<CellPosition> GetAvailableCells(Player?[,] board)
    {
        var cells = new List<CellPosition>();
        for (var row = 0; row < 3; row++)
        {
            for (var col = 0; col < 3; col++)
            {
                if (!board[row, col].HasValue)
                {
                    cells.Add(new CellPosition(row, col));
                }
            }
        }

        return cells;
    }

    public static bool TryGetWinner(Player?[,] board, out Player? winner, out IReadOnlyList<CellPosition> winningCells)
    {
        foreach (var line in WinningLines)
        {
            var first = board[line[0].Row, line[0].Column];
            if (!first.HasValue)
            {
                continue;
            }

            if (line.All(cell => board[cell.Row, cell.Column] == first))
            {
                winner = first;
                winningCells = line;
                return true;
            }
        }

        winner = null;
        winningCells = Array.Empty<CellPosition>();
        return false;
    }

    public static bool IsDraw(Player?[,] board)
    {
        for (var row = 0; row < 3; row++)
        {
            for (var col = 0; col < 3; col++)
            {
                if (!board[row, col].HasValue)
                {
                    return false;
                }
            }
        }

        return true;
    }

    public static CellPosition? FindCriticalMove(Player?[,] board, Player player)
    {
        foreach (var line in WinningLines)
        {
            var playerCount = 0;
            CellPosition? emptyCell = null;

            foreach (var cell in line)
            {
                var value = board[cell.Row, cell.Column];
                if (value == player)
                {
                    playerCount++;
                }
                else if (!value.HasValue)
                {
                    emptyCell = cell;
                }
            }

            if (playerCount == 2 && emptyCell is not null)
            {
                return emptyCell;
            }
        }

        return null;
    }
}
