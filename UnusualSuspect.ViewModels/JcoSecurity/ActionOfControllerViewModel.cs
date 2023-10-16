using UnusualSuspect.ViewModels.Models;

namespace UnusualSuspect.ViewModels.JcoSecurity;

public class ActionOfControllerViewModel
{
    public string Area { set; get; }
    public string Controller { set; get; }
    public string Action { set; get; }
    public string ReturnType { set; get; }
    public string Attributes { set; get; }
    public string FarsiName { get; set; }
    public string EnglishName { get; set; }
    public string ControllerFarsiName { get; set; }
    public string ControllerEnglishName { get; set; }

    public bool Add { get; set; }
}
public class ControllerSearchViewModel : BaseViewModel
{
}
public class SoftwareRoleSearchViewModel : BaseViewModel
{
}
public class AMACtionSearchViewModel : BaseViewModel
{
    public int ControllerId { set; get; }
}
