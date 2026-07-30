namespace TicTacToe.Api.Models;

public sealed record MoveRecord(int MoveNumber, Player Player, int Row, int Column);
