using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School.Entities.Models;

/// <summary>
/// سمت
/// </summary>
public class Post : BaseEntity
{
    [Display(Name = "عنوان سمت")]
    public string PostName { set; get; }
}
