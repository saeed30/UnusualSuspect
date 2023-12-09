using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using UnusualSuspect.Entities.GameModels;
using UnusualSuspect.Services.Contracts;

namespace UnusualSuspect.Admin.Controllers
{
  public class PreGameController : BaseController<PreGameController>
  {
    private readonly IPreGameService preGameService;
    public PreGameController(ILogger<PreGameController> logger, IPreGameService preGameService) : base(logger)
    {
      this.preGameService = preGameService;
    }
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
  }
}
