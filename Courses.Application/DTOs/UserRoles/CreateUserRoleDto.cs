using System;
using System.Collections.Generic;
using System.Text;

namespace Courses.Application.DTOs.UserRoles;

public class CreateUserRoleDto
{
    public int UserId { get; set; }
    public int RoleId { get; set; }
}
