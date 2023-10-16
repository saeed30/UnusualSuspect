using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnusualSuspect.ViewModels.Models;

public class LogObjectSearchViewModel : BaseViewModel
{
    public DateTime? StartDate { set; get; }
    public DateTime? EndDate { set; get; }
    public string AdminUser { set; get; }
    public string ObjectType { set; get; }
}