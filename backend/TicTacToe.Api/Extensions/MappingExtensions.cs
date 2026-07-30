using TicTacToe.Api.Contracts;
using TicTacToe.Api.Models;

namespace TicTacToe.Api.Extensions;

public static class MappingExtensions
{
    public static GameStateResponse ToResponse(this GameSession game, Scoreboard scoreboard)
    {
        var board = new string?[3][];

        for (var row = 0; row < 3; row++)
        {
            board[row] = new string?[3];
            for (var col = 0; col < 3; col++)
            {
                board[row][col] = game.Board[row, col]?.ToString();
            }
        }

        return new GameStateResponse
        {
            GameId = game.Id,
            Mode = game.Mode,
            Difficulty = game.Difficulty,
            Board = board,
            CurrentPlayer = game.CurrentPlayer,
            Status = game.Status,
            Winner = game.Winner,
            WinningCells = game.WinningCells.ToArray(),
            MoveHistory = game.MoveHistory
                .Select(m => new MoveResponse(m.MoveNumber, m.Player, m.Row, m.Column))
                .ToArray(),
            Scoreboard = scoreboard.ToResponse()
        };
    }

    public static ScoreboardResponse ToResponse(this Scoreboard scoreboard)
        => new(scoreboard.XWins, scoreboard.OWins, scoreboard.Draws);
}
