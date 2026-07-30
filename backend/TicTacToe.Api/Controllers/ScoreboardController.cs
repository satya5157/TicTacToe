using Microsoft.AspNetCore.Mvc;
using TicTacToe.Api.Contracts;
using TicTacToe.Api.Services;

namespace TicTacToe.Api.Controllers;

[ApiController]
[Route("api/scoreboard")]
public sealed class ScoreboardController : ControllerBase
{
    private readonly IGameService _gameService;

    public ScoreboardController(IGameService gameService)
    {
        _gameService = gameService;
    }

    [HttpGet]
    public ActionResult<ScoreboardResponse> Get()
        => Ok(_gameService.GetScoreboard());

    [HttpPost("reset")]
    public ActionResult<ScoreboardResponse> Reset()
        => Ok(_gameService.ResetScoreboard());
}
