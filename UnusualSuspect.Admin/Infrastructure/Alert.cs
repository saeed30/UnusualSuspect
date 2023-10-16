using System;

namespace UnusualSuspect.Admin.Infrastructure;

[Serializable]
public class Alert
{
    public const string TempDataKey = "TempDataAlerts";
    public string AlertStyle { get; set; }
    public string Message { get; set; }
    public bool Dismissable { get; set; }
}

[Serializable]
public class AlertStyles
{
    public const string Success = "success";
    public const string Information = "info";
    public const string Warning = "warning";
    public const string Danger = "danger";
}
