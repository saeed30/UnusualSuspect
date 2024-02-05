using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using UnusualSuspect.Admin.Models;
using UnusualSuspect.ApiViewModels.Endpoints.Game;
using UnusualSuspect.ApiViewModels.InnerModels.Game;
using UnusualSuspect.Common.Attribute;
using UnusualSuspect.Entities.GameModels;
using UnusualSuspect.Services.Contracts;
using UnusualSuspect.Services.Services;
using UnusualSuspect.ViewModels.Game;

namespace UnusualSuspect.Admin.Controllers;

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

  [PersianTitle("جزئیات بازی")]
  [ServiceFilter(typeof(UserFilters))]
  public async Task<IActionResult> Details(int id)
  {
    var game = await gameService.GetDetailByIdAsync(id);
    if(!game.Success)
      return NotFound();
    return View(game.Result);
  }
}