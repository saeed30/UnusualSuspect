using UnusualSuspect.ViewModels.Models;
using Microsoft.AspNetCore.Mvc;


namespace UnusualSuspect.ViewComponents.Areas.Components.ViewComponents;

public class PagingResultGrid2ViewComponent : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync(int ResultSum, int PageSize, int Step)
    {
        return View("~/Areas/Components/Views/PagingResultGrid2.cshtml", new PagingResaultSearchViewModel()
        {
            ResultSum = ResultSum,
            PageSize = PageSize,
            Step = Step
        });
    }
}
