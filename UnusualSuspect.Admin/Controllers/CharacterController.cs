using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using UnusualSuspect.Admin.Models;
using UnusualSuspect.Common.Attribute;
using UnusualSuspect.DataLayer;
using UnusualSuspect.Entities.GameModels;
using UnusualSuspect.Services.Contracts;

namespace UnusualSuspect.Admin.Controllers;

public class CharacterController(ILogger<CharacterController> logger,
    ICharacterService characterService,
    IFileService fileService,
    IUnitOfWork uow)
  : BaseController<CharacterController>(logger)
{
  [PersianTitle("لیست کاراکترها")]
  [ServiceFilter(typeof(UserFilters))]
  public IActionResult Index()
  {
    return View();
  }
  public IActionResult Character_Read([DataSourceRequest] DataSourceRequest request)
  {
    IQueryable<CharacterCard> items = characterService.GetAllCharacters();
    DataSourceResult result = items.ToDataSourceResult(request);
    result.Total = items.Count();
    return Json(result);
  }
  [HttpPost]
  public ActionResult Character_Create([DataSourceRequest] DataSourceRequest request, CharacterCard character)
  {
    if (ModelState.IsValid)
    {
      characterService.AddCharacter(character);
      uow.SaveChanges();
    }
    return Json(new[] { character }.ToDataSourceResult(request, ModelState));
  }
  [HttpPost]
  public async Task<ActionResult> Character_Update([DataSourceRequest] DataSourceRequest request, [BindRequired] CharacterCard character)
  {
    if (character != null && ModelState.IsValid)
    {
      var result = await characterService.UpdateCharacterAsync(character, character.OriginalId);
      if (result.Success)
      {
        if (result.Result)
          await uow.SaveChangesAsync();
      }
      else
      {
        ModelState.AddModelError(string.Empty, "Update failed: " + result.MainError.GetDisplay());
        HttpContext.Response.StatusCode = 500;
      }
    }
    return Json(new[] { character }.ToDataSourceResultAsync(request, ModelState));
  }
  [HttpPost]
  public async Task<IActionResult> Character_Destroy([DataSourceRequest] DataSourceRequest request, [BindRequired] CharacterCard character)
  {
    if (character != null)
    {
      characterService.DeleteCharacter(character);
      await uow.SaveChangesAsync();
    }

    // Return an empty result.
    return Json(await new[] { character }.ToDataSourceResultAsync(request, ModelState));
  }
}