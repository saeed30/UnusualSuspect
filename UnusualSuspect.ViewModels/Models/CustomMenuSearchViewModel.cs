using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnusualSuspect.ViewModels.Models;

public class CustomMenuSearchViewModel:BaseViewModel
{
    public int? SoftSectionId { set; get; }

    public int? ParentId { set; get; }
}
