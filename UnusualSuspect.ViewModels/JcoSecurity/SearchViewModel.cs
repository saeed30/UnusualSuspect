using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UnusualSuspect.ViewModels.JcoSecurity;

public class SearchViewModel
{


    public SearchViewModel()
    {
        SortTypeList = new List<SelectListItem>
            {
                new SelectListItem() { Text = "مرتب سازی بر اساس", Value = "" },
                new SelectListItem() { Text = "جدیدترین ", Value = "1" },
                new SelectListItem() { Text = "قدیمی ترین ", Value = "2" }
            };

        PageSizeList = new List<SelectListItem>
            {
                new SelectListItem() { Text = "30 آیتم", Value = "30" },
                new SelectListItem() { Text = "15 آیتم", Value = "15" },
                new SelectListItem() { Text = "60 آیتم", Value = "60" },
                new SelectListItem() { Text = "100 آیتم", Value = "100" }
            };
        SortTypeId = 1;
        PageSize = 30;
    }

    public int PageSize { set; get; }
    public virtual List<SelectListItem> PageSizeList { set; get; }

    [Display(Name = "کلمات کلیدی")]
    public string KeyWord { set; get; }

    public int SchoolId { set; get; }
    [Display(Name = "به ترتیب")]
    public int? SortTypeId { set; get; }
    public virtual List<SelectListItem> SortTypeList { set; get; }

    public int Step { set; get; }

    public int? ParentId { get; set; }
}
