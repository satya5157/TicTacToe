using TicTacToe.Api.Models;

namespace TicTacToe.Api.Services.UndoPolicies;

public interface IUndoPolicy
{
    GameMode Mode { get; }

    int GetMoveCountToUndo(GameSession game);
}
