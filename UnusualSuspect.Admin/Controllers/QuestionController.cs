using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using UnusualSuspect.Admin.Models;
using UnusualSuspect.ApiViewModels.Endpoints.Game;
using UnusualSuspect.ApiViewModels.Enums;
using UnusualSuspect.Common.Attribute;
using UnusualSuspect.Common.Models;
using UnusualSuspect.DataLayer;
using UnusualSuspect.Entities.GameModels;
using UnusualSuspect.Services;
using UnusualSuspect.Services.Contracts;
using UnusualSuspect.ViewModels.Mapper;
using UnusualSuspect.ViewModels.Question;

namespace UnusualSuspect.Admin.Controllers
{
  public class QuestionController(IQuestionService questionService, IUnitOfWork uow) : Controller
  {
    [PersianTitle("پاسخ های پیش فرض")]
    [ServiceFilter(typeof(UserFilters))]

    public IActionResult SetDefaultAnswer()
    {
      return View();
    }
    public IActionResult SetDefaultAnswer_Read([DataSourceRequest] DataSourceRequest request)
    {
      IQueryable<QuestionCharacterCardDefaultAnswer> items = questionService.GetAllDefaultAnswersWithDetails();
      var result = items.OrderByDescending(x => x.Id).ToDataSourceResult(request);
      return Json(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetFirstUnansweredQuestion(CancellationToken cancellationToken = default)
    {
      GetFirstUnansweredQuestionViewmodel? firstUnanswered = await questionService.GetFirstUnanswered(cancellationToken);
      ApiResultCommon<GetFirstUnansweredQuestionViewmodel> result = firstUnanswered == null ?
        new ApiResultCommon<GetFirstUnansweredQuestionViewmodel>(true, ApiResultStatusCode.NotFound, null) :
        new ApiResultCommon<GetFirstUnansweredQuestionViewmodel>(true, ApiResultStatusCode.Success, firstUnanswered);
      return Json(result);
    }

    [ServiceFilter(typeof(UserFilters))]
    [HttpPost]
    public async Task<IActionResult> SetQuestionDefaultAnswer(SetQuestionDefaultAnswerViewmodel model, CancellationToken cancellationToken = default)
    {
      UnusualSuspectServiceResult<bool> result = await questionService.SetDefaultAnswer(model, cancellationToken);
      if (result.Success)
      {
        if (result.Result)
          await uow.SaveChangesAsync(cancellationToken);
        return Json(new ApiResultCommon(true, ApiResultStatusCode.Success));
      }
      return Json(new ApiResultCommon(false,ApiResultStatusCode.LogicError, result.MainError.GetDisplay()));
    }

  }
}
