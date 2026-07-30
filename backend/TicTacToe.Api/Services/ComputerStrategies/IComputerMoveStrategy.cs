using TicTacToe.Api.Models;

namespace TicTacToe.Api.Services.ComputerStrategies;

public interface IComputerMoveStrategy
{
    ComputerDifficulty Difficulty { get; }

    CellPosition ChooseMove(GameSession game);
}
