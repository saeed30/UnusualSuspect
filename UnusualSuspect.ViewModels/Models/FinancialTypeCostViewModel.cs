using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnusualSuspect.ViewModels.Models;

public class FinancialTypeCostViewModel : BaseViewModel
{
    public int? FinancialTypeId { set; get; }
    public string FinancialTypeName { set; get; }

}
