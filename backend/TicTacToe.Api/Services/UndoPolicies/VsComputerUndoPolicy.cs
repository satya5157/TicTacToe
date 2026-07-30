using TicTacToe.Api.Models;

namespace TicTacToe.Api.Services.UndoPolicies;

public sealed class VsComputerUndoPolicy : IUndoPolicy
{
    public GameMode Mode => GameMode.VsComputer;

    public int GetMoveCountToUndo(GameSession game)
        => Math.Min(2, game.MoveHistory.Count);
}
