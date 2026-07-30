using TicTacToe.Api.Models;
using TicTacToe.Api.Services;

namespace TicTacToe.Api.Tests;

public sealed class GameServiceTests
{
    [Fact]
    public void ValidMove_UpdatesBoardAndHistory()
    {
        var service = new GameService();
        var game = service.CreateGame(GameMode.TwoPlayer);

        var updated = service.MakeMove(game.GameId, Player.X, 0, 0);

        Assert.Equal("X", updated.Board[0][0]);
        Assert.Single(updated.MoveHistory);
        Assert.Equal(Player.O, updated.CurrentPlayer);
    }

    [Fact]
    public void InvalidMove_OnOccupiedCell_Throws()
    {
        var service = new GameService();
        var game = service.CreateGame(GameMode.TwoPlayer);
        service.MakeMove(game.GameId, Player.X, 0, 0);

        Assert.Throws<InvalidOperationException>(() => service.MakeMove(game.GameId, Player.O, 0, 0));
    }

    [Fact]
    public void TurnSwitching_AlternatesBetweenPlayers()
    {
        var service = new GameService();
        var game = service.CreateGame(GameMode.TwoPlayer);

        var afterX = service.MakeMove(game.GameId, Player.X, 0, 0);
        var afterO = service.MakeMove(game.GameId, Player.O, 1, 1);

        Assert.Equal(Player.O, afterX.CurrentPlayer);
        Assert.Equal(Player.X, afterO.CurrentPlayer);
    }

    [Fact]
    public void RowWin_IsDetected()
    {
        var service = new GameService();
        var game = service.CreateGame(GameMode.TwoPlayer);

        service.MakeMove(game.GameId, Player.X, 0, 0);
        service.MakeMove(game.GameId, Player.O, 1, 0);
        service.MakeMove(game.GameId, Player.X, 0, 1);
        service.MakeMove(game.GameId, Player.O, 1, 1);
        var result = service.MakeMove(game.GameId, Player.X, 0, 2);

        Assert.Equal(GameStatus.Won, result.Status);
        Assert.Equal(Player.X, result.Winner);
        Assert.Equal(3, result.WinningCells.Count);
    }

    [Fact]
    public void ColumnWin_IsDetected()
    {
        var service = new GameService();
        var game = service.CreateGame(GameMode.TwoPlayer);

        service.MakeMove(game.GameId, Player.X, 0, 0);
        service.MakeMove(game.GameId, Player.O, 0, 1);
        service.MakeMove(game.GameId, Player.X, 1, 0);
        service.MakeMove(game.GameId, Player.O, 1, 1);
        var result = service.MakeMove(game.GameId, Player.X, 2, 0);

        Assert.Equal(GameStatus.Won, result.Status);
        Assert.Equal(Player.X, result.Winner);
    }

    [Fact]
    public void DiagonalWin_IsDetected()
    {
        var service = new GameService();
        var game = service.CreateGame(GameMode.TwoPlayer);

        service.MakeMove(game.GameId, Player.X, 0, 0);
        service.MakeMove(game.GameId, Player.O, 0, 1);
        service.MakeMove(game.GameId, Player.X, 1, 1);
        service.MakeMove(game.GameId, Player.O, 1, 0);
        var result = service.MakeMove(game.GameId, Player.X, 2, 2);

        Assert.Equal(GameStatus.Won, result.Status);
        Assert.Equal(Player.X, result.Winner);
    }

    [Fact]
    public void Draw_IsDetected()
    {
        var service = new GameService();
        var game = service.CreateGame(GameMode.TwoPlayer);

        service.MakeMove(game.GameId, Player.X, 0, 0);
        service.MakeMove(game.GameId, Player.O, 0, 1);
        service.MakeMove(game.GameId, Player.X, 0, 2);
        service.MakeMove(game.GameId, Player.O, 1, 1);
        service.MakeMove(game.GameId, Player.X, 1, 0);
        service.MakeMove(game.GameId, Player.O, 1, 2);
        service.MakeMove(game.GameId, Player.X, 2, 1);
        service.MakeMove(game.GameId, Player.O, 2, 0);
        var result = service.MakeMove(game.GameId, Player.X, 2, 2);

        Assert.Equal(GameStatus.Draw, result.Status);
        Assert.Null(result.Winner);
    }

    [Fact]
    public void ResetGame_ClearsBoardAndHistory_AndKeepsScoreboard()
    {
        var service = new GameService();
        var game = service.CreateGame(GameMode.TwoPlayer);

        service.MakeMove(game.GameId, Player.X, 0, 0);
        service.MakeMove(game.GameId, Player.O, 1, 0);
        service.MakeMove(game.GameId, Player.X, 0, 1);
        service.MakeMove(game.GameId, Player.O, 1, 1);
        service.MakeMove(game.GameId, Player.X, 0, 2);

        var reset = service.ResetGame(game.GameId);

        Assert.Equal(GameStatus.InProgress, reset.Status);
        Assert.Empty(reset.MoveHistory);
        Assert.Equal(Player.X, reset.CurrentPlayer);
        Assert.Equal(1, reset.Scoreboard.XWins);
    }

    [Fact]
    public void Undo_InTwoPlayerMode_RemovesMostRecentMove()
    {
        var service = new GameService();
        var game = service.CreateGame(GameMode.TwoPlayer);

        service.MakeMove(game.GameId, Player.X, 0, 0);
        service.MakeMove(game.GameId, Player.O, 1, 1);

        var undone = service.Undo(game.GameId);

        Assert.Single(undone.MoveHistory);
        Assert.Equal("X", undone.Board[0][0]);
        Assert.Null(undone.Board[1][1]);
        Assert.Equal(Player.O, undone.CurrentPlayer);
    }

    [Fact]
    public void Undo_InComputerMode_RemovesMovePair()
    {
        var service = new GameService();
        var game = service.CreateGame(GameMode.VsComputer);

        var afterMoves = service.MakeMove(game.GameId, Player.X, 0, 0);
        Assert.Equal(2, afterMoves.MoveHistory.Count);

        var undone = service.Undo(game.GameId);

        Assert.Empty(undone.MoveHistory);
        Assert.Equal(Player.X, undone.CurrentPlayer);
    }

    [Fact]
    public void Scoreboard_UpdatesOnCompletionOnlyOnce()
    {
        var service = new GameService();
        var game = service.CreateGame(GameMode.TwoPlayer);

        service.MakeMove(game.GameId, Player.X, 0, 0);
        service.MakeMove(game.GameId, Player.O, 1, 0);
        service.MakeMove(game.GameId, Player.X, 0, 1);
        service.MakeMove(game.GameId, Player.O, 1, 1);
        var won = service.MakeMove(game.GameId, Player.X, 0, 2);

        Assert.Equal(1, won.Scoreboard.XWins);

        Assert.Throws<InvalidOperationException>(() => service.MakeMove(game.GameId, Player.O, 2, 2));
        var scoreboard = service.GetScoreboard();
        Assert.Equal(1, scoreboard.XWins);
    }

    [Fact]
    public void ComputerMoveSelection_PrefersCenterThenBlockThenWin()
    {
        var service = new GameService();
        var game = service.CreateGame(GameMode.VsComputer);

        var afterFirst = service.MakeMove(game.GameId, Player.X, 0, 0);
        Assert.Equal("O", afterFirst.Board[1][1]);

        var afterSecond = service.MakeMove(game.GameId, Player.X, 0, 1);
        Assert.Equal("O", afterSecond.Board[0][2]);

        var final = service.MakeMove(game.GameId, Player.X, 2, 2);
        Assert.Equal(GameStatus.Won, final.Status);
        Assert.Equal(Player.O, final.Winner);
    }

    [Fact]
    public void MoveAfterGameCompletion_IsRejected()
    {
        var service = new GameService();
        var game = service.CreateGame(GameMode.TwoPlayer);

        service.MakeMove(game.GameId, Player.X, 0, 0);
        service.MakeMove(game.GameId, Player.O, 1, 0);
        service.MakeMove(game.GameId, Player.X, 0, 1);
        service.MakeMove(game.GameId, Player.O, 1, 1);
        service.MakeMove(game.GameId, Player.X, 0, 2);

        Assert.Throws<InvalidOperationException>(() => service.MakeMove(game.GameId, Player.O, 2, 2));
    }
}