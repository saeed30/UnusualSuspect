namespace UnusualSuspect.ViewModels.Models;

public class UsersInRoleViewModel
{
    public int UserId { get; set; }
    public int? MemberTypeId { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string PhoneNumber { get; set; }
    
    public string LastName { get; set; }
    public Guid? DocumentGuid { get; set; }
    public bool IsActive { set; get; }
    public string? NickName { get; set; }
    public short? AvatarId { get; set; }

  public string Role { get; set; }
  public string RoleNamesFa { get; set; }

}

