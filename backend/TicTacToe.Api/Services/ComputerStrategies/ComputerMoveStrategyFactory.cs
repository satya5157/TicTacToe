using TicTacToe.Api.Models;

namespace TicTacToe.Api.Services.ComputerStrategies;

public sealed class ComputerMoveStrategyFactory : IComputerMoveStrategyFactory
{
    private readonly IReadOnlyDictionary<ComputerDifficulty, IComputerMoveStrategy> _strategies;

    public ComputerMoveStrategyFactory(IEnumerable<IComputerMoveStrategy> strategies)
    {
        _strategies = strategies.ToDictionary(strategy => strategy.Difficulty, strategy => strategy);
    }

    public IComputerMoveStrategy GetStrategy(ComputerDifficulty difficulty)
    {
        if (_strategies.TryGetValue(difficulty, out var strategy))
        {
            return strategy;
        }

        throw new InvalidOperationException($"No computer strategy is registered for difficulty '{difficulty}'.");
    }
}
