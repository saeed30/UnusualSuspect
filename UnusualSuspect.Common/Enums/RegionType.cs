using System.ComponentModel.DataAnnotations;

namespace UnusualSuspect.Common.Enums;

public enum RegionType : short
{
    /// <summary>
    /// کشور
    /// </summary>
    [Display(Name = "کشور")]
    Country,
    /// <summary>
    /// استان
    /// </summary>
    [Display(Name = "استان")]
    State,
    /// <summary>
    /// شهرستان
    /// </summary>
    [Display(Name = "شهرستان")]
    County,
    /// <summary>
    /// شهر
    /// </summary>
    [Display(Name = "شهر")]
    City,
}
