using TicTacToe.Api.Contracts;
using TicTacToe.Api.Extensions;
using TicTacToe.Api.Models;
using TicTacToe.Api.Services.ComputerStrategies;
using TicTacToe.Api.Services.GameRules;
using TicTacToe.Api.Services.UndoPolicies;

namespace TicTacToe.Api.Services;

public sealed class GameService : IGameService
{
    private readonly Dictionary<Guid, GameSession> _games = new();
    private readonly Scoreboard _scoreboard = new();
    private readonly object _syncRoot = new();
    private readonly IComputerMoveStrategyFactory _computerStrategyFactory;
    private readonly IReadOnlyDictionary<GameMode, IUndoPolicy> _undoPolicies;

    public GameService()
        : this(
            new ComputerMoveStrategyFactory(
                new IComputerMoveStrategy[]
                {
                    new EasyComputerMoveStrategy(),
                    new MediumComputerMoveStrategy(),
                    new HardComputerMoveStrategy()
                }),
            new IUndoPolicy[]
            {
                new TwoPlayerUndoPolicy(),
                new VsComputerUndoPolicy()
            })
    {
    }

    public GameService(
        IComputerMoveStrategyFactory computerStrategyFactory,
        IEnumerable<IUndoPolicy> undoPolicies)
    {
        _computerStrategyFactory = computerStrategyFactory;
        _undoPolicies = undoPolicies.ToDictionary(policy => policy.Mode, policy => policy);
    }

    public GameStateResponse CreateGame(GameMode mode, ComputerDifficulty difficulty = ComputerDifficulty.Medium)
    {
        lock (_syncRoot)
        {
            var game = new GameSession
            {
                Mode = mode,
                Difficulty = mode == GameMode.VsComputer ? difficulty : ComputerDifficulty.Medium
            };

            _games.Add(game.Id, game);
            return game.ToResponse(_scoreboard);
        }
    }

    public GameStateResponse GetGame(Guid gameId)
    {
        lock (_syncRoot)
        {
            var game = GetGameOrThrow(gameId);
            return game.ToResponse(_scoreboard);
        }
    }

    public GameStateResponse MakeMove(Guid gameId, Player player, int row, int column)
    {
        lock (_syncRoot)
        {
            var game = GetGameOrThrow(gameId);
            ValidateMove(game, player, row, column);

            ApplyMove(game, player, row, column);
            EvaluateGameResult(game);

            if (game.Mode == GameMode.VsComputer && game.Status == GameStatus.InProgress)
            {
                var strategy = _computerStrategyFactory.GetStrategy(game.Difficulty);
                var computerMove = strategy.ChooseMove(game);
                ApplyMove(game, Player.O, computerMove.Row, computerMove.Column);
                EvaluateGameResult(game);
            }

            return game.ToResponse(_scoreboard);
        }
    }

    public GameStateResponse Undo(Guid gameId)
    {
        lock (_syncRoot)
        {
            var game = GetGameOrThrow(gameId);

            if (game.Status != GameStatus.InProgress)
            {
                throw new InvalidOperationException("Undo is disabled after game completion (Option A).");
            }

            if (game.MoveHistory.Count == 0)
            {
                throw new InvalidOperationException("There are no moves to undo.");
            }

            var moveCountToRemove = GetUndoPolicy(game.Mode).GetMoveCountToUndo(game);

            game.MoveHistory.RemoveRange(game.MoveHistory.Count - moveCountToRemove, moveCountToRemove);
            RebuildBoardFromHistory(game);

            return game.ToResponse(_scoreboard);
        }
    }

    public GameStateResponse ResetGame(Guid gameId)
    {
        lock (_syncRoot)
        {
            var game = GetGameOrThrow(gameId);

            Array.Clear(game.Board);
            game.MoveHistory.Clear();
            game.WinningCells.Clear();
            game.Winner = null;
            game.Status = GameStatus.InProgress;
            game.CurrentPlayer = Player.X;
            game.IsResultCounted = false;

            return game.ToResponse(_scoreboard);
        }
    }

    public ScoreboardResponse GetScoreboard()
    {
        lock (_syncRoot)
        {
            return _scoreboard.ToResponse();
        }
    }

    public ScoreboardResponse ResetScoreboard()
    {
        lock (_syncRoot)
        {
            _scoreboard.XWins = 0;
            _scoreboard.OWins = 0;
            _scoreboard.Draws = 0;
            return _scoreboard.ToResponse();
        }
    }

    private GameSession GetGameOrThrow(Guid gameId)
    {
        if (!_games.TryGetValue(gameId, out var game))
        {
            throw new KeyNotFoundException($"Game '{gameId}' was not found.");
        }

        return game;
    }

    private static void ValidateMove(GameSession game, Player player, int row, int column)
    {
        if (row is < 0 or > 2 || column is < 0 or > 2)
        {
            throw new InvalidOperationException("Move is outside of the board.");
        }

        if (game.Status != GameStatus.InProgress)
        {
            throw new InvalidOperationException("Game is already completed.");
        }

        if (game.Mode == GameMode.VsComputer && player != Player.X)
        {
            throw new InvalidOperationException("In computer mode, the human player must be X.");
        }

        if (game.CurrentPlayer != player)
        {
            throw new InvalidOperationException("Move by wrong player.");
        }

        if (game.Board[row, column].HasValue)
        {
            throw new InvalidOperationException("Cell is already occupied.");
        }
    }

    private static void ApplyMove(GameSession game, Player player, int row, int column)
    {
        game.Board[row, column] = player;
        game.MoveHistory.Add(new MoveRecord(game.MoveHistory.Count + 1, player, row, column));
        game.CurrentPlayer = player == Player.X ? Player.O : Player.X;
    }

    private void EvaluateGameResult(GameSession game)
    {
        game.Winner = null;
        game.WinningCells.Clear();

        if (BoardAnalyzer.TryGetWinner(game.Board, out var winner, out var winningCells))
        {
            game.Status = GameStatus.Won;
            game.Winner = winner;
            game.WinningCells.AddRange(winningCells);
            UpdateScoreboardOnCompletion(game);
            return;
        }

        if (BoardAnalyzer.IsDraw(game.Board))
        {
            game.Status = GameStatus.Draw;
            UpdateScoreboardOnCompletion(game);
        }
        else
        {
            game.Status = GameStatus.InProgress;
        }
    }

    private void UpdateScoreboardOnCompletion(GameSession game)
    {
        if (game.IsResultCounted)
        {
            return;
        }

        if (game.Status == GameStatus.Won)
        {
            if (game.Winner == Player.X)
            {
                _scoreboard.XWins++;
            }
            else
            {
                _scoreboard.OWins++;
            }
        }
        else if (game.Status == GameStatus.Draw)
        {
            _scoreboard.Draws++;
        }

        game.IsResultCounted = true;
    }

    private IUndoPolicy GetUndoPolicy(GameMode mode)
    {
        if (_undoPolicies.TryGetValue(mode, out var policy))
        {
            return policy;
        }

        throw new InvalidOperationException($"No undo policy is registered for mode '{mode}'.");
    }

    private static void RebuildBoardFromHistory(GameSession game)
    {
        Array.Clear(game.Board);

        for (var i = 0; i < game.MoveHistory.Count; i++)
        {
            var move = game.MoveHistory[i];
            game.Board[move.Row, move.Column] = move.Player;
            game.MoveHistory[i] = move with { MoveNumber = i + 1 };
        }

        game.Winner = null;
        game.WinningCells.Clear();
        game.Status = GameStatus.InProgress;
        game.IsResultCounted = false;

        if (game.Mode == GameMode.VsComputer)
        {
            game.CurrentPlayer = Player.X;
        }
        else
        {
            game.CurrentPlayer = game.MoveHistory.Count % 2 == 0 ? Player.X : Player.O;
        }
    }
}
