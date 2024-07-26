namespace UnusualSuspect.ViewModels.Identity;

public class CurrentUserViewModel(int userId, string username)
{
  public int UserId { get; set; } = userId;
  public string Username { get; set; } = username;
}