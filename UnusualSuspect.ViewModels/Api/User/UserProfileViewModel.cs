using System.ComponentModel.DataAnnotations;

namespace UnusualSuspect.ViewModels.Api.User;

public class UserProfileViewModel
{
    [Display(Name = "نام")]
    public string FirstName { get; set; }

    [Display(Name = "نام خانوادگی")]
    public string LastName { get; set; }

    public string ImageUrl { get; set; }
    public string State { get; set; }
    public string Role { get; set; }
}
