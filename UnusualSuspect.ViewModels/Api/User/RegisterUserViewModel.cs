using UnusualSuspect.ViewModels.Identity;

namespace UnusualSuspect.ViewModels.Api.User;

public class RegisterUserViewModel
{
    public AccessToken AccessToken { get; set; }
    public UserProfileViewModel User { get; set; }
}
