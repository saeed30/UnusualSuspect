using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnusualSuspect.ViewModels.Api.InputParameters;

public class ListValueParameters<TKey>
{
    [Required(ErrorMessage = "پارامترهای ارسالی الزامی می باشد")]
    public IEnumerable<TKey> KeyValue { get; set; }
}
