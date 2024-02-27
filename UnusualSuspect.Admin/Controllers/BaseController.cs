using UnusualSuspect.Admin.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace UnusualSuspect.Admin.Controllers;

public class BaseController<T>(ILogger<T> logger) : Controller
{
    protected JsonResult ReturnJsonResult(bool isSuccess = true, string? message = null, object? extraData = null)
    {
        if (extraData == null)
            return Json(new { success = isSuccess, msg = message });
        return Json(new
        {
            success = isSuccess,
            msg = message,
            data = JsonSerializer.Serialize(extraData)
        });
    }

    #region alert
    public void Success(string message, bool dismissable = false)
    {
        AddAlert(AlertStyles.Success, message, dismissable);
    }

    public void Information(string message, bool dismissable = false)
    {
        AddAlert(AlertStyles.Information, message, dismissable);
    }

    public void Warning(string message, bool dismissable = false)
    {
        AddAlert(AlertStyles.Warning, message, dismissable);
    }

    public void Danger(string message, bool dismissable = false)
    {
        AddAlert(AlertStyles.Danger, message, dismissable);
    }

    private void AddAlert(string alertStyle, string message, bool dismissable)
    {
        var alerts = TempData.ContainsKey(Alert.TempDataKey)
            ? JsonSerializer.Deserialize<List<Alert>>(TempData[Alert.TempDataKey].ToString())
            : new List<Alert>();

        alerts.Add(new Alert
        {
            AlertStyle = alertStyle,
            Message = message,
            Dismissable = dismissable
        });

        TempData[Alert.TempDataKey] = JsonSerializer.Serialize(alerts);
    }

    #endregion alert

}
