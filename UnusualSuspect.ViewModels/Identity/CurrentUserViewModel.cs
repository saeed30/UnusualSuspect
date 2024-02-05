
namespace UnusualSuspect.ViewModels.Identity;

public class CurrentUserViewModel
{
  public CurrentUserViewModel(int userId, string username)
  {
    UserId = userId;
    Username = username;
  }
  public int UserId { get; set; }
  public string Username { get; set; }
}