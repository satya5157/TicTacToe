using TicTacToe.Api.Models;

namespace TicTacToe.Api.Contracts;

public sealed class MoveRequest
{
    public Player Player { get; set; }

    public int Row { get; set; }

    public int Column { get; set; }
}
