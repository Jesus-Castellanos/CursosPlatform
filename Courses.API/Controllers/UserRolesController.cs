using Courses.Application.DTOs.UserRoles;
using Courses.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Courses.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserRolesController : ControllerBase
{
    private readonly IUserRolesService _userRolesService;

    public UserRolesController(
        IUserRolesService userRolesService)
    {
        _userRolesService = userRolesService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserRolesDto>>> GetAll()
    {
        var userRoles =
            await _userRolesService.GetAllAsync();

        return Ok(userRoles);
    }

    [HttpPost]
    public async Task<ActionResult<UserRolesDto>> AssignRole(
        [FromBody] CreateUserRoleDto dto)
    {
        try
        {
            var userRole =
                await _userRolesService.AssignRoleAsync(dto);

            return Ok(userRole);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new
            {
                message = ex.Message
            });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new
            {
                message = ex.Message
            });
        }
    }

    [HttpDelete("{userId:int}/{roleId:int}")]
    public async Task<IActionResult> RemoveRole(
        int userId,
        int roleId)
    {
        var removed =
            await _userRolesService.RemoveRoleAsync(
                userId,
                roleId);

        if (!removed)
        {
            return NotFound(new
            {
                message =
                    $"El usuario {userId} no tiene asignado el rol {roleId}."
            });
        }

        return NoContent();
    }
}