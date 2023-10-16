using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnusualSuspect.Entities.Models;

public class HomeMenu : BaseEntity
{

    [Display(Name = "عنوان"), StringLength(256)]
    public string Title { get; set; }


    [Display(Name = "متن")]
    public string Text { get; set; }


    [Display(Name = "لینک")]
    public string Link { get; set; }


    [Display(Name = "آیکون")]
    public string Icon { get; set; }


    [Display(Name = "اولویت")]
    public int Priority { get; set; }


    /// فعال و غیرفعال بودن
    public bool IsActive { set; get; }


    [NotMapped]
    public IFormFile ImageFile { set; get; }


}
