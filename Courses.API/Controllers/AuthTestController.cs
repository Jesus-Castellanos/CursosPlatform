using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Courses.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthTestController : ControllerBase
{
    [HttpGet("public")]
    public IActionResult Public()
    {
        return Ok(new
        {
            message = "Endpoint público funcionando."
        });
    }

    [Authorize]
    [HttpGet("private")]
    public IActionResult Private()
    {
        return Ok(new
        {
            message = "JWT válido. Acceso autorizado.",
            user = User.Identity?.Name,
            claims = User.Claims.Select(c => new
            {
                c.Type,
                c.Value
            })
        });
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("admin")]
    public IActionResult Admin()
    {
        return Ok(new
        {
            message = "Acceso autorizado para Admin.",
            user = User.Identity?.Name
        });
    }
}