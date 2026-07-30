using TicTacToe.Api.Models;

namespace TicTacToe.Api.Services.ComputerStrategies;

public interface IComputerMoveStrategyFactory
{
    IComputerMoveStrategy GetStrategy(ComputerDifficulty difficulty);
}
