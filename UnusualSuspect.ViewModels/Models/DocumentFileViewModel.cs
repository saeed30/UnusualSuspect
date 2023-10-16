using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnusualSuspect.ViewModels.Models;

public class DocumentFileViewModel
{
    public int MemberId { set; get; }
    public int MemberTypeId { set; get; }
    public string TableName { set; get; }
    public int DocumentTypeId { set; get; } 

}
