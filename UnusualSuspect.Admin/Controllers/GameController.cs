using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using UnusualSuspect.Admin.Models;
using UnusualSuspect.Common.Attribute;
using UnusualSuspect.Entities.GameModels;
using UnusualSuspect.Services.Contracts;
using UnusualSuspect.Services.Services;

namespace UnusualSuspect.Admin.Controllers
{
  public class GameController(ILogger<GameController> logger, IGameService gameService) : BaseController<GameController>(logger)
  {
    [PersianTitle("لیست بازی ها")]
    [ServiceFilter(typeof(UserFilters))]
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
