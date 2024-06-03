namespace UnusualSuspect.ViewModels.Models;

public class DocumentSearchViewModel : BaseViewModel
{
    public bool? Active { set; get; }
    public int? DocumentGroupId { set; get; }
    public string DocumentGroupName { get; set; }
    public DateTime? StartDate { set; get; }
    public DateTime? EndDate { set; get; }
    public string Lang { get; set; }
}
public class DocumentGroupSearchViewModel : BaseViewModel
{
}
public class DocumentCommentSearchViewModel : BaseViewModel
{
}
