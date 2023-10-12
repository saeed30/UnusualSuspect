using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School.Entities.Models;

/// <summary>
/// نوع تراکنش مالی 
/// </summary>
public class FinancialType : BaseEntity
{
    [Display(Name = "نوع مبلغ")]
    public string FinancialTypeName { set; get; }


}

public enum FinancialTypes
{
    هزینه_بررسی_اولیه_پرونده = 1,
    هزینه_بازرسی = 2,
    ورودیه = 3,
    حق_عضویت = 4,
    کارمزد_سامانه = 5,
}
