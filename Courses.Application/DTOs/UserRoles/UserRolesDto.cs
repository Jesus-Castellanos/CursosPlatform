using System;
using System.Collections.Generic;
using System.Text;

namespace Courses.Application.DTOs.UserRoles;

public class UserRolesDto
{
    public int UserId { get; set; }
    public string? UserName { get; set; }

    public int RoleId { get; set; }
    public string?RoleName { get; set; }

    public DateTime AssignedAt { get; set; }
}
