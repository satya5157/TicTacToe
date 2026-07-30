using TicTacToe.Api.Models;

namespace TicTacToe.Api.Services.UndoPolicies;

public sealed class TwoPlayerUndoPolicy : IUndoPolicy
{
    public GameMode Mode => GameMode.TwoPlayer;

    public int GetMoveCountToUndo(GameSession game)
        => Math.Min(1, game.MoveHistory.Count);
}
