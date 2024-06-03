namespace UnusualSuspect.ViewModels.Models;

public class CustomerSearchViewModel : BaseViewModel
{

    public bool? Active { set; get; }
    public DateTime? StartDate { set; get; }
    public DateTime? EndDate { set; get; }
    public string Lang { get; set; }

}
