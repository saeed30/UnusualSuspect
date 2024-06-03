using Microsoft.AspNetCore.Mvc.Rendering;
using Nancy.Json;
using System.Text.RegularExpressions;
using System.Web;

namespace UnusualSuspect.Common;

public class HelperCommon
{
    public static string ReturnMessageException(Exception e)
    {
        return e.InnerException != null ? e.InnerException.Message : e.Message;
    }

    public static TEntity ShallowCopyEntity<TEntity>(TEntity source) where TEntity : class, new()
    {
        var DataTypeList = new string[] { "Byte", "SByte", "Int32", "UInt32", "Int16", "UInt16", "Int64", "UInt64", "Single", "Double", "Char", "Boolean", "String", "Decimal", "DateTime" };
        var sourceProperties = typeof(TEntity)
                                .GetProperties()
                                .Where(p => p.CanRead && p.CanWrite && DataTypeList.Any(x => p.PropertyType.FullName.Contains(x)));
        var newObj = new TEntity();
        foreach (var property in sourceProperties)
        {
            property.SetValue(newObj, property.GetValue(source, null), null);
        }
        return newObj;
    }

    public static string ShallowCopyEntityToString<TEntity>(TEntity source) where TEntity : class, new()
    {
        var DataTypeList = new string[] { "Byte", "SByte", "Int32", "UInt32", "Int16", "UInt16", "Int64", "UInt64", "Single", "Double", "Char", "Boolean", "String", "Decimal", "DateTime" };
        var sourceProperties = typeof(TEntity)
                                .GetProperties()
                                .Where(p => p.CanRead && p.CanWrite && DataTypeList.Any(x => p.PropertyType.FullName.Contains(x)) && !p.CustomAttributes.Any(c => c.AttributeType.Name == "NotMappedAttribute"));
        string TempSt = "";
        foreach (var property in sourceProperties)
        {
            TempSt += property.Name + ":" + property.GetValue(source, null) + ",";
        }
        return TempSt.Substring(0, TempSt.Length - 1);
    }
    public static string Reverse(string s)
    {
        char[] charArray = s.ToCharArray();
        Array.Reverse(charArray);
        return new string(charArray);
    }
    //public static DateTime? ParseDateTime(string faDate)
    //{
    //    try
    //    {
    //        faDate = faDate.Fa2En();
    //        string[] YMD = faDate.Substring(0, 10).Split('/');
    //        string[] HMS = new string[3];
    //        if (faDate.Length > 11)
    //        {
    //            HMS = faDate.Substring(10, 8).Split(':');
    //            return new PersianDateTime(int.Parse(YMD[0]), int.Parse(YMD[1]), int.Parse(YMD[2]), int.Parse(HMS[0]), int.Parse(HMS[1]), int.Parse(HMS[2])).ToDateTime();
    //        }
    //        else
    //        {
    //            return new PersianDateTime(int.Parse(YMD[0]), int.Parse(YMD[1]), int.Parse(YMD[2])).ToDateTime();
    //        }
    //    }
    //    catch
    //    {
    //        return null;
    //    }
    //}
    public static List<SelectListItem> SortTypeList()
    {
        var SortTypeList = new List<SelectListItem>();
        SortTypeList.Add(new SelectListItem() { Value = "1", Text = "مرتبط ترین" });
        SortTypeList.Add(new SelectListItem() { Value = "2", Text = "ارزانترین" });
        SortTypeList.Add(new SelectListItem() { Value = "3", Text = "گرانترین" });
        SortTypeList.Add(new SelectListItem() { Value = "4", Text = "جدیدترین" });
        return SortTypeList;
    }
    public static bool MobileDeviceDetection(string UserAgent)
    {
        return UserAgent.Contains("Mobile");
    }

    public static string StripHTML(string input)
    {
        return input != null ? Regex.Replace(input, "<.*?>", String.Empty).Replace("\r\n", string.Empty) : "";
    }
    /// <summary>
    /// تبدیل پارامترهای کوئری استرینگ به آبجکت
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="Url"></param>
    /// <returns></returns>
    public static T GeTObjebtFromQueryString<T>(string Url)
    {
        var dict = HttpUtility.ParseQueryString(Url.Split('?')[1]);
        var json = new JavaScriptSerializer().Serialize(
                            dict.AllKeys.ToDictionary(k => k, k => dict[k]));
        return new JavaScriptSerializer().Deserialize<T>(json);


    }
    //public static bool SendMessageToWhatsApp()
    //{
    //    string from = "989118590955";
    //    string to = "989357289420";//Sender Mobile
    //    string msg ="salam";

    //    WhatsApp wa = new WhatsApp(from, "BnXk*******B0=", "NickName", true, true);

    //    wa.OnConnectSuccess += () =>
    //    {
    //        //MessageBox.Show("Connected to whatsapp...");

    //        wa.OnLoginSuccess += (phoneNumber, data) =>
    //        {
    //            wa.SendMessage(to, msg);
    //            //MessageBox.Show("Message Sent...");
    //        };

    //        wa.OnLoginFailed += (data) =>
    //        {
    //            //MessageBox.Show("Login Failed : {0}", data);
    //        };

    //        wa.Login();
    //    };

    //    wa.OnConnectFailed += (ex) =>
    //    {
    //        //MessageBox.Show("Connection Failed...");
    //    };

    //    wa.Connect();
    //    return true;
    //}
}
