using System.Security.Claims;
using Courses.Application.DTOs.LessonProgress;
using Courses.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Courses.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LessonProgressController : ControllerBase
{
    private readonly ILessonProgressService _progressService;

    public LessonProgressController(
        ILessonProgressService progressService)
    {
        _progressService = progressService;
    }

    // =====================================================
    // GET ALL
    // Admin e Instructor pueden consultar todo
    // =====================================================

    [HttpGet]
    [Authorize(Roles = "Admin,Instructor")]
    public async Task<
        ActionResult<IEnumerable<LessonProgressDto>>> GetAll()
    {
        var progress =
            await _progressService.GetAllAsync();

        return Ok(progress);
    }

    // =====================================================
    // GET BY ID
    // Admin/Instructor -> cualquier progreso
    // Student -> solamente el suyo
    // =====================================================

    [HttpGet("{id:int}")]
    public async Task<ActionResult<LessonProgressDto>> GetById(
        int id)
    {
        var progress =
            await _progressService.GetByIdAsync(id);

        if (progress is null)
        {
            return NotFound(new
            {
                message =
                    $"No se encontró el progreso con ID {id}."
            });
        }

        // Admin e Instructor pueden consultar cualquiera.
        if (User.IsInRole("Admin") ||
            User.IsInRole("Instructor"))
        {
            return Ok(progress);
        }

        // Student solamente puede consultar su propio progreso.
        var currentUserId = GetCurrentUserId();

        if (currentUserId is null)
        {
            return Unauthorized();
        }

        if (progress.UserId != currentUserId.Value)
        {
            return Forbid();
        }

        return Ok(progress);
    }

    // =====================================================
    // CREATE
    // Admin/Instructor -> pueden crear para cualquier usuario
    // Student -> solamente para sí mismo
    // =====================================================

    [HttpPost]
    [Authorize(Roles = "Admin,Instructor,Student")]
    public async Task<ActionResult<LessonProgressDto>> Create(
        [FromBody] CreateLessonProgressDto dto)
    {
        try
        {
            // Si es Student, ignoramos el UserId enviado
            // y usamos el usuario autenticado.
            if (User.IsInRole("Student"))
            {
                var currentUserId = GetCurrentUserId();

                if (currentUserId is null)
                {
                    return Unauthorized();
                }

                dto.UserId = currentUserId.Value;
            }

            var progress =
                await _progressService.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = progress.LessonProgressId },
                progress);
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

    // =====================================================
    // UPDATE
    // Admin/Instructor -> cualquier progreso
    // Student -> solamente el suyo
    // =====================================================

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin,Instructor,Student")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] UpdateLessonProgressDto dto)
    {
        try
        {
            // Primero obtenemos el progreso existente
            // para saber a qué usuario pertenece.
            var existing =
                await _progressService.GetByIdAsync(id);

            if (existing is null)
            {
                return NotFound(new
                {
                    message =
                        $"No se encontró el progreso con ID {id}."
                });
            }

            // Student solamente puede modificar el suyo.
            if (User.IsInRole("Student"))
            {
                var currentUserId = GetCurrentUserId();

                if (currentUserId is null)
                {
                    return Unauthorized();
                }

                if (existing.UserId != currentUserId.Value)
                {
                    return Forbid();
                }
            }

            var updated =
                await _progressService.UpdateAsync(
                    id,
                    dto);

            if (!updated)
            {
                return NotFound(new
                {
                    message =
                        $"No se encontró el progreso con ID {id}."
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

    // =====================================================
    // DELETE
    // Solo Admin e Instructor
    // =====================================================

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin,Instructor")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted =
            await _progressService.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound(new
            {
                message =
                    $"No se encontró el progreso con ID {id}."
            });
        }

        return NoContent();
    }

    // =====================================================
    // JWT USER ID
    // =====================================================

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