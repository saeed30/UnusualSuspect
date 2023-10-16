using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnusualSuspect.Common.Utilities;

public static class StringExtensions
{
    public static string ToEnglishNumber(this object text)
    {
        string str = (string)text;
        string vInt = "۱۲۳۴۵۶۷۸۹۰";
        char[] mystring = str.ToCharArray(0, str.Length);
        var newStr = string.Empty;
        for (var i = 0; i <= (mystring.Length - 1); i++)
            if (vInt.IndexOf(mystring[i]) == -1)
                newStr += mystring[i];
            else
            {
                newStr += char.GetNumericValue(mystring[i]);
            }
        return newStr;
    }

    public static bool HasValue(this string value, bool ignoreWhiteSpace = true)
    {
        return ignoreWhiteSpace ? !string.IsNullOrWhiteSpace(value) : !string.IsNullOrEmpty(value);
    }

    public static int ToInt(this string value)
    {
        return Convert.ToInt32(value);
    }
    public static bool IsImage(this string value)
    {
        List<string> file = new List<string>() { ".JPG", ".JPEG", ".PNG", ".GIF" };
        return file.Contains(Path.GetExtension(value).ToUpper());
    }

    public static decimal ToDecimal(this string value)
    {
        return Convert.ToDecimal(value);
    }

    public static double ToDouble(this string value)
    {
        return Convert.ToDouble(value);
    }
    public static string ToNumeric(this int value)
    {
        return value.ToString("N0");
    }

    public static string ToNumeric(this decimal value)
    {
        return value.ToString("N0");
    }
    public static string BytesToString(this int byteCount)
    {
        return BytesToStringLong(byteCount);
    }
    public static string BytesToStringLong(this long byteCount)
    {
        string[] suf = { "B", "KB", "MB", "GB", "TB", "PB", "EB" };
        if (byteCount == 0)
            return "0" + suf[0];
        long bytes = Math.Abs(byteCount);
        int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
        double num = Math.Round(bytes / Math.Pow(1024, place), 1);
        return (Math.Sign(byteCount) * num).ToString() + suf[place];
    }
    public static string En2Fa(this string str)
    {
        return str.Replace("0", "۰")
            .Replace("1", "۱")
            .Replace("2", "۲")
            .Replace("3", "۳")
            .Replace("4", "۴")
            .Replace("5", "۵")
            .Replace("6", "۶")
            .Replace("7", "۷")
            .Replace("8", "۸")
            .Replace("9", "۹");
    }

    public static string Fa2En(this string str)
    {
        return str.Replace("۰", "0")
            .Replace("۱", "1")
            .Replace("۲", "2")
            .Replace("۳", "3")
            .Replace("۴", "4")
            .Replace("۵", "5")
            .Replace("۶", "6")
            .Replace("۷", "7")
            .Replace("۸", "8")
            .Replace("۹", "9")
            //iphone numeric
            .Replace("٠", "0")
            .Replace("١", "1")
            .Replace("٢", "2")
            .Replace("٣", "3")
            .Replace("٤", "4")
            .Replace("٥", "5")
            .Replace("٦", "6")
            .Replace("٧", "7")
            .Replace("٨", "8")
            .Replace("٩", "9");
    }

    public static string FixPersianChars(this string str)
    {
        return str.Replace("ﮎ", "ک")
            .Replace("ﮏ", "ک")
            .Replace("ﮐ", "ک")
            .Replace("ﮑ", "ک")
            .Replace("ك", "ک")
            .Replace("ي", "ی")
            .Replace(" ", " ")
            .Replace("‌", " ")
            .Replace("ھ", "ه");//.Replace("ئ", "ی");
    }

    public static string CleanString(this string str)
    {
        return str.Trim().FixPersianChars().Fa2En().NullIfEmpty();
    }

    public static string NullIfEmpty(this string str)
    {
        return str?.Length == 0 ? null : str;
    }
    public static bool IsNull(this string str)
    {
        return string.IsNullOrEmpty(str);
    }

    public static string AddQueryString(string url, string queryStringFragment)
    {
        int i = url.IndexOf("?");
        if (i > 0 && i < url.Length - 1)
            url += "&" + queryStringFragment;
        else
            url += "?" + queryStringFragment;
        return url;
    }
}
