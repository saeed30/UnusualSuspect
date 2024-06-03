using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace UnusualSuspect.Common.Attribute;

[AttributeUsage(AttributeTargets.All, Inherited = true)]
public class PersianTitleAttribute :System.Attribute
{
    private readonly string _title;
    public string Title
    {
        get { return _title; }
    }

    public PersianTitleAttribute(string title)
    {
        _title = title;
    }
}



[AttributeUsage(AttributeTargets.All, Inherited = true)]
public class AddAttribute :System.Attribute
{
    private readonly bool _add;
    public bool Add
    {
        get { return _add; }
    }

    public AddAttribute(bool add)
    {
        _add = add;
    }
}

public static class Extensions
{
    public static string GetPersianTitle(this MemberInfo target)
    {
        return target.GetCustomAttributes(typeof(PersianTitleAttribute), true)
            .Cast<PersianTitleAttribute>().Select(d => d.Title)
            .SingleOrDefault() ?? target.Name;
    }
    public static string GetDisplayName(this MemberInfo target)
    {
        return target.GetCustomAttributes(typeof(DisplayAttribute), true)
            .Cast<DisplayAttribute>().Select(d => d.Name)
            .SingleOrDefault() ?? target.Name;
    }
}
