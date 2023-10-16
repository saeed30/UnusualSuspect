using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace UnusualSuspect.Common.Utilities;

public static class PersianDate
{
    public static DateTime dt { get { return DateTime.Now; } }
    private static PersianCalendar objPersianCalender = new PersianCalendar();

    private static string[] Mounths = new string[12]
{
  "فروردین",
  "اردیبهشت",
  "خرداد",
  "تیر",
  "مرداد",
  "شهریور",
  "مهر",
  "ابان",
  "اذر",
  "دی",
  "بهمن",
  "اسفند"
};

    private static string[] Days = new string[7]
{
  "شنبه",
  "یک شنبه",
  "دوشنبه",
  "سه شنبه",
  "چهار شنبه",
  "پنج شنبه",
  "جمعه"
};

    public static int mounth()
    {
        return objPersianCalender.GetMonth(dt);
    }
    public static int mounth(DateTime d)
    {
        return objPersianCalender.GetMonth(d);
    }
    public static string Mounth()
    {
        return ((object)Mounths[mounth() - 1]).ToString();
    }
    public static string Mounth(DateTime dt)
    {
        return ((object)Mounths[mounth(dt) - 1]).ToString();
    }
    public static string Mounth(int Index)
    {
        return Mounths[Index - 1];
    }

    public static int Year()
    {
        return objPersianCalender.GetYear(dt);
    }
    public static int Year(DateTime date)
    {
        return objPersianCalender.GetYear(date);
    }
    public static string DayOfWeek()
    {
        return GetDayofWeek(dt);
    }
    public static int DayofMounth()
    {
        return objPersianCalender.GetDayOfMonth(dt);
    }
    public static int DayofMounth(DateTime t)
    {
        return objPersianCalender.GetDayOfMonth(t);
    }
    public static string Today()
    {
        return DayOfWeek() + ", " + DayofMounth() + " " + ((object)Mounths[mounth() - 1]).ToString() + " " + Year() + " " + GetTime(dt);
    }

    public static string GetPersianDateAndTime(DateTime dt)
    {
        return GetDayofWeek(dt) + ", " + objPersianCalender.GetDayOfMonth(dt) + " " + Mounth() + " " + Year();
    }


    public static string GetWeekDay(DateTime dt)
    {
        return GetDayofWeek(dt) + " " + GetNowDate();
    }

    public static object mounth(object date)
    {
        throw new NotImplementedException();
    }

    public static bool IsHolidaysDay(DateTime datetime)
    {
        var day = objPersianCalender.GetDayOfWeek(datetime);
        return day == System.DayOfWeek.Friday || day == System.DayOfWeek.Thursday;
    }



    public static string GetDayofWeek(DateTime datetime)
    {
        string str = "";
        switch (objPersianCalender.GetDayOfWeek(datetime))
        {
            case System.DayOfWeek.Sunday:
                str = "یک شنبه";
                break;

            case System.DayOfWeek.Monday:
                str = "دوشنبه";
                break;

            case System.DayOfWeek.Tuesday:
                str = "سه شنبه";
                break;

            case System.DayOfWeek.Wednesday:
                str = "چهار شنبه";
                break;

            case System.DayOfWeek.Thursday:
                str = "پنج شنبه";
                break;

            case System.DayOfWeek.Friday:
                str = "جمعه";
                break;

            case System.DayOfWeek.Saturday:
                str = "شنبه";
                break;
        }
        return str;
    }

    public static int GetDayofWeekInt(DateTime datetime)
    {
        switch (objPersianCalender.GetDayOfWeek(datetime))
        {
            case System.DayOfWeek.Sunday: return 1;
            case System.DayOfWeek.Monday: return 2;
            case System.DayOfWeek.Tuesday: return 3;
            case System.DayOfWeek.Wednesday: return 4;
            case System.DayOfWeek.Thursday: return 5;
            case System.DayOfWeek.Friday: return 6;
            case System.DayOfWeek.Saturday: return 0;
        }
        return 0;
    }

    public static string GetNowDate()
    {
        string m = mounth().ToString();
        string day = DayofMounth().ToString();
        day = int.Parse(day) < 10 ? "0" + day : day;
        m = int.Parse(m) < 10 ? "0" + m : m;
        return Year() + "/" + m + "/" + day;
    }
    public static string GetNowDateWhitDash()
    {
        string m = mounth().ToString();
        string day = DayofMounth().ToString();
        day = int.Parse(day) < 10 ? "0" + day : day;
        m = int.Parse(m) < 10 ? "0" + m : m;
        return Year() + "-" + m + "-" + day;
    }
    public static string GetFullDate()
    {
        return GetNowDate() + " " + GetNowTime();
    }
    public static string GetNowTime()
    {
        return DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;
    }
    public static string GetNowTimeShortWhitAmPm()
    {
        return DateTime.Now.Hour + ":" + DateTime.Now.Minute + " " + DateTime.Now.ToString("tt", CultureInfo.InvariantCulture);
    }
    public static string GetNowTimeShortWhitAmPm(DateTime dt)
    {
        string b = dt.Hour == 0 ? "00" : dt.Hour.ToString() + ":";
        b += dt.Minute == 0 ? "00" : dt.Minute.ToString();
        b += " " + dt.ToString("tt", CultureInfo.InvariantCulture);
        return b;
    }
    public static string GetTime(DateTime dt)
    {
        return dt.Hour + ":" + dt.Minute + ":" + dt.Second;
    }
    public static string GetOneDaysAgo()
    {
        DateTime DayAgo = DateTime.Now.AddDays(-1);
        string m = objPersianCalender.GetMonth(DayAgo).ToString();
        string day = objPersianCalender.GetDayOfMonth(DayAgo).ToString();
        string Years = objPersianCalender.GetYear(DayAgo).ToString();
        day = int.Parse(day) < 10 ? "0" + day : day;
        m = int.Parse(m) < 10 ? "0" + m : m;
        return Years + "/" + m + "/" + day;
    }

    public static string GetNowNormalDate()
    {
        string m = mounth().ToString();
        string day = DayofMounth().ToString();
        return Year() + "/" + m + "/" + day;
    }

    public static int GetDay(DateTime item)
    {
        return objPersianCalender.GetDayOfMonth(item);
    }

    public static string GetDate(int Years, int Month, int Day)
    {
        string m, day;
        day = Day < 10 ? "0" + Day : Day.ToString();
        m = Month < 10 ? "0" + Month : Month.ToString();
        return Years + "/" + m + "/" + day;
    }
    public static DateTime GetDate(int Years, int Month, int Day, int H, int M, int S)
    {
        return objPersianCalender.ToDateTime(Years, Month, Day, H, M, S, 0);
    }
    public static DateTime GetDate(string date)
    {
        string[] TokenNow = date.Split('/');
        int day = int.Parse(TokenNow[2]);
        int month = int.Parse(TokenNow[1]);
        int years = int.Parse(TokenNow[0]);
        return objPersianCalender.ToDateTime(years, month, day, 0, 0, 0, 0, 0);
    }
    public static int DayInmonth(DateTime date)
    {
        int _month = mounth(date);
        int _years = Year(date);
        return objPersianCalender.GetDaysInMonth(_years, _month);
    }
    public static string GetNormalDate(string Date)
    {
        string[] Token = Date.Split('/');
        long Day = long.Parse(Token[2]);
        long Month = long.Parse(Token[1]);
        return Token[0] + "/" + Month + "/" + Day;
    }

    public static bool ValidDate(string Date)
    {
        string[] TokenNow = GetNowDate().Split('/');
        long Day = long.Parse(TokenNow[2]);
        long Month = long.Parse(TokenNow[1]);
        long Years = long.Parse(TokenNow[0]);
        string[] TokenEnterded = Date.Split('/');
        long DayE = long.Parse(TokenEnterded[2]);
        long MonthE = long.Parse(TokenEnterded[1]);
        long YearsE = long.Parse(TokenEnterded[0]);
        return YearsE != Years ? true : MonthE != Month ? true : DayE != Day ? true : false;
    }
    /// <summary>
    /// فرمت تاریخ وارد شده معتبر می باشد یا نه
    /// </summary>
    /// <param name="Date"></param>
    /// <returns></returns>
    public static bool ValidationDate(string Date)
    {
        try
        {
            string[] TokenEnterded = Date.Split('/');
            int DayE = int.Parse(TokenEnterded[2]);
            int MonthE = int.Parse(TokenEnterded[1]);
            int YearsE = int.Parse(TokenEnterded[0]);
            objPersianCalender.ToDateTime(YearsE, MonthE, DayE, 0, 0, 0, 0);
            return true;
        }
        catch
        {
            return false;
        }
    }
    public static string GetNextMonthDate(string Date)
    {
        string[] TokenEnterded = Date.Split('/');
        long DayE = long.Parse(TokenEnterded[2]);
        long MonthE = long.Parse(TokenEnterded[1]);
        long YearsE = long.Parse(TokenEnterded[0]);
        YearsE = MonthE == 12 ? ++YearsE : YearsE;
        MonthE = MonthE == 12 ? 1 : ++MonthE;
        string day = DayE < 10 ? "0" + DayE : DayE.ToString();
        string m = MonthE < 10 ? "0" + MonthE : MonthE.ToString();
        return YearsE.ToString() + "/" + m + "/" + day;
    }

    public static string GetTime()
    {
        return DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute;
    }
    public static int GetDay(string SDate, string EDate)
    {
        List<string> sd = SDate.Split('/').ToList();
        List<string> ed = EDate.Split('/').ToList();
        if (int.Parse(sd[1]) == int.Parse(ed[1]))
        {
            return int.Parse(ed[2]) - int.Parse(sd[2]);
        }
        else if (int.Parse(ed[1]) > int.Parse(sd[1]))
        {
            int m = ((int.Parse(ed[1]) - int.Parse(sd[1]))) * 30;
            int b = int.Parse(sd[2]) - int.Parse(ed[2]);
            int i = int.Parse(sd[1]) <= 6 ? 1 : int.Parse(sd[1]) > 6 && int.Parse(sd[1]) < 12 ? 0 : -1;
            return (m - b) + i;
        }
        else
        {
            int m = int.Parse(sd[1]);
            m = m <= 6 ? 31 : m > 6 && m <= 11 ? 30 : 29;
            m = m - int.Parse(sd[2]);
            int b = m + int.Parse(ed[2]);
            return b;
        }
    }
    public static string GetDate(DateTime dt)
    {
        System.Globalization.PersianCalendar x = new System.Globalization.PersianCalendar();
        string years = x.GetYear(dt) + "";
        string month = x.GetMonth(dt) + "";
        string day = x.GetDayOfMonth(dt) + "";
        if (int.Parse(day) < 10)
            day = "0" + day;
        if (int.Parse(month) < 10)
            month = "0" + month;
        return years + "/" + month + "/" + day;
    }
    public static string GetFullDate(DateTime dt)
    {
        System.Globalization.PersianCalendar x = new System.Globalization.PersianCalendar();
        string years = x.GetYear(dt) + "";
        string month = x.GetMonth(dt) + "";
        string day = x.GetDayOfMonth(dt) + "";
        if (int.Parse(day) < 10)
            day = "0" + day;
        if (int.Parse(month) < 10)
            month = "0" + month;
        return years + "/" + month + "/" + day + " " + dt.Hour + ":" + dt.Minute;
    }
    public static string GetDateTimeToPersianDate(DateTime dt)
    {
        System.Globalization.PersianCalendar x = new System.Globalization.PersianCalendar();
        string years = x.GetYear(dt) + "";
        string month = ((object)Mounths[x.GetMonth(dt) - 1]).ToString();
        string day = x.GetDayOfMonth(dt) + "";
        string days = GetDayofWeek(dt);
        return days + " " + day + " " + month + " " + years;
    }
    public static string[] GetDateNoZero(DateTime dt)
    {
        System.Globalization.PersianCalendar x = new System.Globalization.PersianCalendar();
        string years = x.GetYear(dt) + "";
        string month = x.GetMonth(dt) + "";
        string day = x.GetDayOfMonth(dt) + "";
        return (years + "/" + month + "/" + day).Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
    }
    public static string FirstPageDate()
    {
        return DayofMounth() + " " + Mounth() + "," + Year();
    }


    /// <summary>
    /// آبان 1, ۱۳۹۷ ۴:۰۵ ق.ظ
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    public static string CustomDatetime(DateTime date, bool wtime)
    {
        System.Globalization.PersianCalendar x = new System.Globalization.PersianCalendar();

        string day = x.GetDayOfMonth(date).ToString();
        string month = Mounth(date);
        string y = x.GetYear(date).ToString();

        return day + " " + month + "," + y + (wtime ? " " + date.Hour + ":" + date.Minute : "");

    }

    public static string ToPersianDateTime(this DateTime? v, bool Time)
    {
        if (!v.HasValue) return "";
        return ToPersianDateTime(v.Value, Time);
    }
    public static string ToPersianDateTime(this DateTime v, bool Time)
    {
        if (Time)
            return PersianDate.GetFullDate(v);
        return PersianDate.GetDate(v);
    }

    public static DateTime ToMiladiDateTime(this string v)
    {
        return GetDate(v);
    }

    public static DateTime? ToDateTime(DateTime? selectedDate)
    {
        if (selectedDate == null) return null;
        return objPersianCalender.ToDateTime(selectedDate.Value.Year, selectedDate.Value.Month, selectedDate.Value.Day, 0, 0, 0, 0);
    }

    public static bool TryToDateTime(string dateFa, bool Required, out DateTime? dateTime)
    {
        dateTime = null;
        if (Required && ( string.IsNullOrEmpty(dateFa) || dateFa.Length != 10))
            return false;
        PersianDateTime persianDateTime = PersianDateTime.Parse(dateFa);
        dateTime = persianDateTime.ToDateTime();
        return true;
    }
    public static string GetPersianDatetimeDigitOnly()
    {
        return GetFullDate().Replace("/", "").Replace(":", "");
    }
}