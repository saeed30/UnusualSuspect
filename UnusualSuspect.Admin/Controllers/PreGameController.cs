using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using UnusualSuspect.Admin.Models;
using UnusualSuspect.Common.Attribute;
using UnusualSuspect.Entities.GameModels;
using UnusualSuspect.Services.Contracts;

namespace UnusualSuspect.Admin.Controllers;

public class PreGameController(ILogger<PreGameController> logger, IPreGameService preGameService)
  : BaseController<PreGameController>(logger)
{
  [PersianTitle("لیست گروه های قبل از بازی")]
  [ServiceFilter(typeof(UserFilters))]
  public IActionResult Index()
  {
    return View();
  }
  public IActionResult ActivePregames_Read([DataSourceRequest] DataSourceRequest request)
  {
    IQueryable<PreGameGroup> items = preGameService.GetAllPreGameGroupsWithDetailsWaitingForGame();
    var result = items.OrderByDescending(x => x.Id).ToDataSourceResult(request);
    return Json(result);
  }

  [PersianTitle("جزئیات گروه قبل بازی")]
  [ServiceFilter(typeof(UserFilters))]
  public async Task<IActionResult> Details(int id, CancellationToken cancellationToken = default)
  {
    var model = await preGameService.GetPreGameGroupViewModel(id, cancellationToken);
    if (!model.Success)
      return NotFound();
    return View(model.Result);
  }
}