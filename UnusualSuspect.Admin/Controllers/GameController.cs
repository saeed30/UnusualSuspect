using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using UnusualSuspect.Admin.Models;
using UnusualSuspect.ApiViewModels.Endpoints.LocalOnly;
using UnusualSuspect.ApiViewModels.Enums;
using UnusualSuspect.Common.Attribute;
using UnusualSuspect.Common.Models;
using UnusualSuspect.Entities.GameModels;
using UnusualSuspect.Services.Contracts;
using UnusualSuspect.ViewModels.Question;

namespace UnusualSuspect.Admin.Controllers;

public class GameController(ILogger<GameController> logger,
  IGameService gameService,
  IApiCallService apiCallService) : BaseController<GameController>(logger)
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
  public async Task<IActionResult> Details(int id, CancellationToken cancellationToken = default)
  {
    var game = await gameService.GetDetailByIdAsync(id, cancellationToken);
    if (!game.Success)
      return NotFound();
    return View(game.Result);
  }

  [ServiceFilter(typeof(UserFilters))]
  [HttpPost]
  public async Task<IActionResult> ChangeGameStatus(ChangeGameStateRequest model,
    CancellationToken cancellationToken = default)
  {
    var result = await apiCallService.ChangeGameStateAsync(model, User.Identity.Name);
    if (result.Success)
      return Json(result.Result);
    return Json(new ApiResultCommon(false, ApiResultStatusCode.LogicError, result.MainError.GetDisplay()));
  }

}