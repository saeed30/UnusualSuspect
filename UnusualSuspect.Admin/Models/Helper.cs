using UnusualSuspect.ViewModels.JcoSecurity;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace UnusualSuspect.Admin.Models;

public class Helper
{

    public static List<ActionOfControllerViewModel> GetAllActionsOfController()
    {
        Assembly asm = Assembly.GetExecutingAssembly();

        var controlleractionlist = asm.GetTypes()
                .Where(type => typeof(Controller).IsAssignableFrom(type))
                .SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
                .Where(m => m.CustomAttributes.Any(v => v.AttributeType.Name == "PersianTitleAttribute") && !m.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), true).Any() && !m.DeclaringType.FullName.Contains("Areas"))
                .Select(x => new ActionOfControllerViewModel
                {
                    Area = "AdminPanel",
                    ControllerFarsiName = x.DeclaringType.CustomAttributes.FirstOrDefault(v => v.AttributeType.Name == "PersianTitleAttribute") != null ? x.DeclaringType.CustomAttributes.First(v => v.AttributeType.Name == "PersianTitleAttribute").ConstructorArguments[0].Value.ToString() : "",
                    ControllerEnglishName = x.DeclaringType.CustomAttributes.FirstOrDefault(v => v.AttributeType.Name == "PersianTitleAttribute") != null ? x.DeclaringType.CustomAttributes.First(v => v.AttributeType.Name == "PersianTitleAttribute").ConstructorArguments[0].Value.ToString() : "",
                    Controller = x.DeclaringType.Name.Replace("Controller", ""),
                    Action = x.Name,
                    ReturnType = x.ReturnType.Name,
                    Attributes = string.Join(",", x.GetCustomAttributes().Select(a => a.GetType().Name.Replace("Attribute", ""))),
                    FarsiName = x.CustomAttributes.FirstOrDefault(v => v.AttributeType.Name == "PersianTitleAttribute") != null ? x.CustomAttributes.First(v => v.AttributeType.Name == "PersianTitleAttribute").ConstructorArguments[0].Value.ToString() : "",
                    EnglishName = x.CustomAttributes.FirstOrDefault(v => v.AttributeType.Name == "PersianTitleAttribute") != null ? x.CustomAttributes.First(v => v.AttributeType.Name == "PersianTitleAttribute").ConstructorArguments[0].Value.ToString() : ""
                })
                .OrderBy(x => x.Area).ThenBy(x => x.Controller).ToList();
        return controlleractionlist;
    }

    public static bool CheckViewedItem(string itemName, int id, HttpRequest _request)
    {
        if (_request.Cookies[itemName + id] != null && _request.Cookies[itemName + id] == id.ToString())
        {
            return true;
        }
        return false;
    }
    public static void UpdateViewCountItem(string itemName, int id,  HttpResponse _Response)
    {
        CookieOptions option = new CookieOptions();
        option.Expires = DateTime.Now.AddDays(10);
        _Response.Cookies.Append(itemName+id, id.ToString(), option);
    }
}
