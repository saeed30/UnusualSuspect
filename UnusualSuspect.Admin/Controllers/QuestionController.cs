using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using UnusualSuspect.Admin.Models;
using UnusualSuspect.ApiViewModels.Enums;
using UnusualSuspect.Common.Attribute;
using UnusualSuspect.Common.Models;
using UnusualSuspect.DataLayer;
using UnusualSuspect.Entities.GameModels;
using UnusualSuspect.Services;
using UnusualSuspect.Services.Contracts;
using UnusualSuspect.Services.Services;
using UnusualSuspect.ViewModels.Question;

namespace UnusualSuspect.Admin.Controllers;

public class QuestionController(IQuestionService questionService, IUnitOfWork uow) : Controller
{
  #region QuestionList
  [PersianTitle("لیست سوال ها")]
  [ServiceFilter(typeof(UserFilters))]
  public IActionResult Index()
  {
    return View();
  }
  public IActionResult Question_Read([DataSourceRequest] DataSourceRequest request)
  {
    IQueryable<Question> items = questionService.GetAllQuestions();
    DataSourceResult result = items.ToDataSourceResult(request);
    return Json(result);
  }
  [HttpPost]
  public ActionResult Question_Create([DataSourceRequest] DataSourceRequest request, Question model)
  {
    if (ModelState.IsValid)
    {
      questionService.AddQuestion(model);
      uow.SaveChanges();
    }
    return Json(new[] { model }.ToDataSourceResult(request, ModelState));
  }
  [HttpPost]
  public async Task<ActionResult> Question_Update([DataSourceRequest] DataSourceRequest request, [BindRequired] Question model)
  {
    if (model != null && ModelState.IsValid)
    {
      questionService.UpdateQuestion(model);
      await uow.SaveChangesAsync();
    }
    return Json(new[] { model }.ToDataSourceResultAsync(request, ModelState));
  }
  [HttpPost]
  public async Task<IActionResult> Question_Destroy([DataSourceRequest] DataSourceRequest request, [BindRequired] short id)
  {
      questionService.DeleteQuestionById(id);
      await uow.SaveChangesAsync();

    // Return an empty result.
    return Json(await new[] { id }.ToDataSourceResultAsync(request, ModelState));
  }
  #endregion

  #region DefaultAnswerList
  [PersianTitle("پاسخ های پیش فرض")]
  [ServiceFilter(typeof(UserFilters))]

  public IActionResult SetDefaultAnswer()
  {
    return View();
  }

  [HttpPost]
  public async Task<IActionResult> SetDefaultAnswer_Destroy([DataSourceRequest] DataSourceRequest request, int id)
  {
    if (ModelState.IsValid)
    {
      questionService.DeleteQuestionCharacterCardDefaultAnswer(id);
      await uow.SaveChangesAsync();
    }

    // Return an empty result.
    return Json(await new[] { id }.ToDataSourceResultAsync(request, ModelState));
  }
  public IActionResult SetDefaultAnswer_Read([DataSourceRequest] DataSourceRequest request)
  {
    IQueryable<QuestionCharacterCardDefaultAnswer> items = questionService.GetAllDefaultAnswersWithDetails();
    var result = items.ToDataSourceResult(request);
    return Json(result);
  }

  [HttpGet]
  public async Task<IActionResult> GetFirstUnansweredQuestion(CancellationToken cancellationToken = default)
  {
    GetFirstUnansweredQuestionViewmodel? firstUnanswered = await questionService.GetFirstUnanswered(cancellationToken);
    ApiResultCommon<GetFirstUnansweredQuestionViewmodel> result = firstUnanswered == null ?
      new ApiResultCommon<GetFirstUnansweredQuestionViewmodel>(true, ApiResultStatusCode.NotFound) :
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
    return Json(new ApiResultCommon(false, ApiResultStatusCode.LogicError, result.MainError.GetDisplay()));
  }
  #endregion

}