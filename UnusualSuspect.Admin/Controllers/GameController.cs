using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using UnusualSuspect.Entities.GameModels;
using UnusualSuspect.Services.Contracts;
using UnusualSuspect.Services.Services;

namespace UnusualSuspect.Admin.Controllers
{
  public class GameController :   BaseController<GameController>
  {
    private readonly IGameService gameService;
    public GameController(ILogger<GameController> logger, IGameService gameService) : base(logger)
    {
      this.gameService = gameService;
    }

    public IActionResult Index()
    {
      return View();
    }
    public IActionResult ActiveGames_Read([DataSourceRequest] DataSourceRequest request)
    {
      IQueryable<Game> items = gameService.GetAllActiveGamesWithGameType();
      var result = items.OrderByDescending(x => x.Id).ToDataSourceResult(request);
      return Json(result);
    }

  }
}
