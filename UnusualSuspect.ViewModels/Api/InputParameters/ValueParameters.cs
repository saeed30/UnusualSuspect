using System.ComponentModel.DataAnnotations;

namespace UnusualSuspect.ViewModels.Api.InputParameters;

public class ValueParameters<TKey>
{
    [Required(ErrorMessage ="پارامتر ارسالی الزامی می باشد")]
    public TKey KeyValue { get; set; }
}
