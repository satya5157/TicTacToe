using TicTacToe.Api.Models;

namespace TicTacToe.Api.Contracts;

public sealed record MoveResponse(int MoveNumber, Player Player, int Row, int Column);
