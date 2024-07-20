
namespace UnusualSuspect.Common.Extensions;

public static class IntExtensions
{
  public static short? ToShort(this int value)
  {
    if (value < Int16.MinValue || value > Int16.MaxValue)
      return null;
    return (short)value;
  }
}