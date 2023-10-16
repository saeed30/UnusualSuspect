using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    public string Role { get; set; }
    public string RoleNamesFa { get; set; }
    
}

