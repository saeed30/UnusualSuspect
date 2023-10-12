using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School.Entities.Models;

/// <summary>
/// شهر 
/// </summary>
public  class City : BaseEntity
{
    public string CityName { set; get; }

    public int ProvinceId { get; set; }
    [ForeignKey("ProvinceId")]
    public virtual Province Province { get; set; }

}
