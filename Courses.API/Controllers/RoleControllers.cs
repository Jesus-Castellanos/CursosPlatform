using Courses.Application.DTOs.Roles;
using Courses.Application.Interfaces;
using Courses.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Courses.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RolesController : ControllerBase
{
    private readonly IRoleServices _rolesService;

    public RolesController(IRoleServices rolesService)
    {
        _rolesService = rolesService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RoleDto>>> GetAll()
    {
        var roles = await _rolesService.GetAllAsync();

        return Ok(roles);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<RoleDto>> GetById(int id)
    {
        var role = await _rolesService.GetByIdAsync(id);

        if (role is null)
        {
            return NotFound(new
            {
                message = $"No se encontró el rol con ID {id}."
            });
        }

        return Ok(role);
    }

    [HttpPost]
    public async Task<ActionResult<RoleDto>> Create(
        [FromBody] CreateRoleDto createRoleDto)
    {
        try
        {
            var role = await _rolesService.CreateAsync(
                createRoleDto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = role.RoleId },
                role);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
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

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] UpdateRoleDto updateRoleDto)
    {
        try
        {
            var updated = await _rolesService.UpdateAsync(
                id,
                updateRoleDto);

            if (!updated)
            {
                return NotFound(new
                {
                    message =
                        $"No se encontró el rol con ID {id}."
                });
            }

            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
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

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var deleted = await _rolesService.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound(new
                {
                    message =
                        $"No se encontró el rol con ID {id}."
                });
            }

            return NoContent();
        }
        catch (Exception)
        {
            return Conflict(new
            {
                message =
                    "No se puede eliminar este rol porque puede estar siendo utilizado por usuarios."
            });
        }
    }
}