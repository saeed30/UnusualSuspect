using PersianDate.Standard;

namespace UnusualSuspect.Common;

public static class Extentions
{
    /// <summary>
    /// تبدیل تاریخ و زمان میلادی به فارسی
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string ToPersianDateTime(this DateTime? value)
    {
        if (value == null)
            return null;
        else
        {
            return value?.ToFa("f");
        }
    }

    /// <summary>
    /// تبدیل تاریخ و زمان میلادی به فارسی
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string ToPersianDateTime(this DateTime value)
    {
        return value.ToFa("f");
    }
    /// <summary>
    ///  تبدیل تاریخ  میلادی به فارسی
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string ToPersianDate(this DateTime? value)
    {
        if (value == null)
            return null;
        else
        {
            return value?.ToFa();
        }
    }
    /// <summary>
    ///  تبدیل تاریخ  میلادی به فارسی
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string ToPersianDate(this DateTime value)
    {
        if (value == null)
            return null;
        else
        {
            return value.ToFa();
        }
    }

    /// <summary>
    /// جدا کردن ارقارم به صورت سه تایی
    /// </summary>
    /// <param name="persianStr"></param>
    /// <returns></returns>
    public static string SplitThreeDigit(this string persianStr)
    {
        if (persianStr.Contains("."))
        {
            var t = persianStr.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            return long.Parse(t[0]).ToString("#,#") + "." + t[1];
        }
        return $"{long.Parse(persianStr).ToString("#,#")} تومان ";
    }

    /// <summary>
    /// جدا کردن ارقارم به صورت سه تایی
    /// </summary>
    /// <param name="persianStr"></param>
    /// <returns></returns>
    public static string SplitThreeDigit(this long persianStr)
    {
        if (persianStr == 0)
            return null;
        var Temp = persianStr.ToString();
        if (Temp.Contains("."))
        {
            var t = Temp.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            return long.Parse(t[0]).ToString("#,#") + "." + t[1];
        }
        return $"{long.Parse(Temp).ToString("#,#")} ریال ";
    }
    /// <summary>
    /// جدا کردن ارقارم به صورت سه تایی
    /// </summary>
    /// <param name="persianStr"></param>
    /// <returns></returns>
    public static string SplitThreeDigit(this long? persianStr)
    {
        if (persianStr == null) return null;
        var Temp = persianStr.ToString();
        if (Temp.Contains("."))
        {
            var t = Temp.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            return long.Parse(t[0]).ToString("#,#") + "." + t[1];
        }
        return $"{long.Parse(Temp).ToString("#,#")} تومان ";
    }
    /// <summary>
    /// جدا کردن ارقارم به صورت سه تایی
    /// </summary>
    /// <param name="persianStr"></param>
    /// <returns></returns>
    public static string SplitThreeDigit(this double persianStr)
    {
        if (persianStr == 0)
            return null;
        var Temp = persianStr.ToString();
        if (Temp.Contains("."))
        {
            var t = Temp.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            return long.Parse(t[0]).ToString("#,#") + "." + t[1];
        }
        return $"{long.Parse(Temp).ToString("#,#")} تومان ";
    }
    /// <summary>
    /// جدا کردن ارقارم به صورت سه تایی
    /// </summary>
    /// <param name="persianStr"></param>
    /// <returns></returns>
    public static string SplitThreeDigitNoUnit(this double persianStr)
    {
        if (persianStr == 0)
            return "0";
        var Temp = persianStr.ToString();
        if (Temp.Contains("."))
        {
            var t = Temp.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            return long.Parse(t[0]).ToString("#,#") + "." + t[1];
        }
        return $"{long.Parse(Temp).ToString("#,#")}";
    }
}

