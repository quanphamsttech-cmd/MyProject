using MyProject.Roles.Dto;
using System.Collections.Generic;

namespace MyProject.Web.Models.Users;

public class UserListViewModel
{
    public IReadOnlyList<RoleDto> Roles { get; set; }
}
