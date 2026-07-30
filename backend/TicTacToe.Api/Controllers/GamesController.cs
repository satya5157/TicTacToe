using Microsoft.AspNetCore.Mvc;
using TicTacToe.Api.Contracts;
using TicTacToe.Api.Services;

namespace TicTacToe.Api.Controllers;

[ApiController]
[Route("api/games")]
public sealed class GamesController : ControllerBase
{
    private readonly IGameService _gameService;

    public GamesController(IGameService gameService)
    {
        _gameService = gameService;
    }

    [HttpPost]
    public ActionResult<GameStateResponse> CreateGame([FromBody] CreateGameRequest? request)
    {
        var mode = request?.Mode ?? Models.GameMode.TwoPlayer;
        var difficulty = request?.Difficulty ?? Models.ComputerDifficulty.Medium;
        var response = _gameService.CreateGame(mode, difficulty);
        return CreatedAtAction(nameof(GetGame), new { id = response.GameId }, response);
    }

    [HttpGet("{id:guid}")]
    public ActionResult<GameStateResponse> GetGame(Guid id)
    {
        try
        {
            return Ok(_gameService.GetGame(id));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost("{id:guid}/moves")]
    public ActionResult<GameStateResponse> MakeMove(Guid id, [FromBody] MoveRequest request)
    {
        try
        {
            return Ok(_gameService.MakeMove(id, request.Player, request.Row, request.Column));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("{id:guid}/undo")]
    public ActionResult<GameStateResponse> Undo(Guid id)
    {
        try
        {
            return Ok(_gameService.Undo(id));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("{id:guid}/reset")]
    public ActionResult<GameStateResponse> Reset(Guid id)
    {
        try
        {
            return Ok(_gameService.ResetGame(id));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}
