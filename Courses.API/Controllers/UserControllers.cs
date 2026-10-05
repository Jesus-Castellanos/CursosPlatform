using Courses.Application.DTOs.Users;
using Courses.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Courses.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class UserControllers : ControllerBase
{
    public readonly IUserService _userService;

    public UserControllers(IUserService userService)
    {
        _userService = userService;
    }

    // GET: api/UserControllers
    // Solo Admin puede consultar todos los usuarios.
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetAll()
    {
        var users = await _userService.GetAllAsync();

        return Ok(users);
    }

    // GET: api/UserControllers/5
    // Solo Admin puede consultar un usuario.
    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserDto>> GetById(int id)
    {
        var user = await _userService.GetByIdAsync(id);

        if (user is null)
        {
            return NotFound(new
            {
                message = $"No se encontró el usuario con Id {id}."
            });
        }

        return Ok(user);
    }

    // POST: api/UserControllers
    // Solo Admin puede crear usuarios.
    [HttpPost]
    public async Task<ActionResult<UserDto>> Create(
        [FromBody] CreateUserDto createUserDto)
    {
        try
        {
            var user = await _userService.CreateAsync(
                createUserDto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = user.Id },
                user);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new
            {
                message = ex.Message
            });
        }
    }

    // PUT: api/UserControllers/5
    // Solo Admin puede actualizar usuarios.
    [HttpPut("{id:int}")]
    public async Task<ActionResult<UserDto>> Update(
        int id,
        [FromBody] UpdateUserDto updateUserDto)
    {
        try
        {
            var user = await _userService.UpdateAsync(
                id,
                updateUserDto);

            if (user is null)
            {
                return NotFound(new
                {
                    message = $"No se encontró el usuario con Id {id}."
                });
            }

            return Ok(user);
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
        var deleted = await _userService.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound(new
            {
                message = $"No se encontró el usuario con Id {id}."
            });
        }

        return NoContent();
    }
}