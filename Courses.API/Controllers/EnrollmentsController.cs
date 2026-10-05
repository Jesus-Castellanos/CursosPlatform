using System.Security.Claims;
using Courses.Application.DTOs.Enrollments;
using Courses.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Courses.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EnrollmentsController : ControllerBase
{
    private readonly IEnrollmentsService _enrollmentsService;

    public EnrollmentsController(
        IEnrollmentsService enrollmentsService)
    {
        _enrollmentsService = enrollmentsService;
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Instructor")]
    public async Task<
        ActionResult<IEnumerable<EnrollmentDto>>> GetAll()
    {
        var enrollments =
            await _enrollmentsService.GetAllAsync();

        return Ok(enrollments);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<EnrollmentDto>> GetById(
        int id)
    {
        var enrollment =
            await _enrollmentsService.GetByIdAsync(id);

        if (enrollment is null)
        {
            return NotFound(new
            {
                message =
                    $"No se encontró la inscripción con ID {id}."
            });
        }

        // Admin e Instructor pueden consultar cualquier inscripción
        if (User.IsInRole("Admin") ||
            User.IsInRole("Instructor"))
        {
            return Ok(enrollment);
        }

        // Student solamente puede consultar su propia inscripción
        var currentUserId = GetCurrentUserId();

        if (currentUserId is null)
        {
            return Unauthorized();
        }

        if (enrollment.UserId != currentUserId.Value)
        {
            return Forbid();
        }

        return Ok(enrollment);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Instructor")]
    public async Task<ActionResult<EnrollmentDto>> Create(
        [FromBody] CreateEnrollmentDto dto)
    {
        try
        {
            var enrollment =
                await _enrollmentsService.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = enrollment.EnrollmentId },
                enrollment);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
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

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin,Instructor")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] UpdateEnrollmentDto dto)
    {
        try
        {
            var updated =
                await _enrollmentsService.UpdateAsync(
                    id,
                    dto);

            if (!updated)
            {
                return NotFound(new
                {
                    message =
                        $"No se encontró la inscripción con ID {id}."
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
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted =
            await _enrollmentsService.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound(new
            {
                message =
                    $"No se encontró la inscripción con ID {id}."
            });
        }

        return NoContent();
    }

    private int? GetCurrentUserId()
    {
        var claim =
            User.FindFirst(ClaimTypes.NameIdentifier);

        if (claim is null)
        {
            return null;
        }

        return int.TryParse(
            claim.Value,
            out var userId)
            ? userId
            : null;
    }
}