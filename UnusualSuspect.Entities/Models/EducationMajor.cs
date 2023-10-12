using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School.Entities.Models;

/// <summary>
/// رشته تحصیلی
/// </summary>
public  class EducationMajor : BaseEntity
{


    public string EducationMajorName { set; get; }


}
