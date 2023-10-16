using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnusualSuspect.ViewModels.Models;

public class PagingResaultSearchViewModel
{
    public int PageSize { set; get; }
    public int ResultSum { set; get; }
    public int Step { set; get; }
}
