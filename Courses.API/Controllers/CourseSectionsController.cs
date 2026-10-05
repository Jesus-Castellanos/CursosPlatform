using Courses.Application.DTOs.CourseSections;
using Courses.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Courses.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CourseSectionsController : ControllerBase
{
    private readonly ICourseSectionsService _sectionsService;
    private readonly ICoursesService _coursesService;

    public CourseSectionsController(
        ICourseSectionsService sectionsService,
        ICoursesService coursesService)
    {
        _sectionsService = sectionsService;
        _coursesService = coursesService;
    }

    [HttpGet]
    public async Task<
        ActionResult<IEnumerable<CourseSectionDto>>> GetAll()
    {
        var sections =
            await _sectionsService.GetAllAsync();

        // Admin e Instructor pueden ver todas las secciones.
        if (User.IsInRole("Admin") ||
            User.IsInRole("Instructor"))
        {
            return Ok(sections);
        }

        // Student solamente puede ver secciones
        // de cursos publicados y activos.
        var courses =
            await _coursesService.GetAllAsync();

        var visibleCourseIds = courses
            .Where(x => x.IsPublished && x.IsActive)
            .Select(x => x.CourseId)
            .ToHashSet();

        var visibleSections = sections
            .Where(x => visibleCourseIds.Contains(x.CourseId));

        return Ok(visibleSections);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CourseSectionDto>> GetById(
        int id)
    {
        var section =
            await _sectionsService.GetByIdAsync(id);

        if (section is null)
        {
            return NotFound(new
            {
                message =
                    $"No se encontró la sección con ID {id}."
            });
        }

        // Admin e Instructor pueden consultar cualquier sección.
        if (User.IsInRole("Admin") ||
            User.IsInRole("Instructor"))
        {
            return Ok(section);
        }

        // Student solamente puede consultar
        // secciones de cursos publicados y activos.
        var course =
            await _coursesService.GetByIdAsync(
                section.CourseId);

        if (!course.IsPublished ||
            !course.IsActive)
        {
            return NotFound(new
            {
                message =
                    $"No se encontró la sección con ID {id}."
            });
        }

        return Ok(section);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Instructor")]
    public async Task<ActionResult<CourseSectionDto>> Create(
        [FromBody] CreateCourseSectionDto dto)
    {
        try
        {
            // Instructor solo puede crear secciones
            // dentro de sus propios cursos.
            if (User.IsInRole("Instructor"))
            {
                var currentUserId = GetCurrentUserId();

                if (currentUserId is null)
                {
                    return Unauthorized();
                }

                var course =
                    await _coursesService.GetByIdAsync(
                        dto.CourseId);

                if (course.CreatedByUserId != currentUserId.Value)
                {
                    return Forbid();
                }
            }

            var section =
                await _sectionsService.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = section.SectionId },
                section);
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
        [FromBody] UpdateCourseSectionDto dto)
    {
        try
        {
            var section =
                await _sectionsService.GetByIdAsync(id);

            if (section is null)
            {
                return NotFound(new
                {
                    message =
                        $"No se encontró la sección con ID {id}."
                });
            }

            // Instructor solo puede modificar
            // secciones de sus propios cursos.
            if (User.IsInRole("Instructor"))
            {
                var currentUserId = GetCurrentUserId();

                if (currentUserId is null)
                {
                    return Unauthorized();
                }

                var course =
                    await _coursesService.GetByIdAsync(
                        section.CourseId);

                if (course.CreatedByUserId != currentUserId.Value)
                {
                    return Forbid();
                }
            }

            var updated =
                await _sectionsService.UpdateAsync(id, dto);

            if (!updated)
            {
                return NotFound(new
                {
                    message =
                        $"No se encontró la sección con ID {id}."
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

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin,Instructor")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var section =
                await _sectionsService.GetByIdAsync(id);

            if (section is null)
            {
                return NotFound(new
                {
                    message =
                        $"No se encontró la sección con ID {id}."
                });
            }

            // Instructor solo puede eliminar
            // secciones de sus propios cursos.
            if (User.IsInRole("Instructor"))
            {
                var currentUserId = GetCurrentUserId();

                if (currentUserId is null)
                {
                    return Unauthorized();
                }

                var course =
                    await _coursesService.GetByIdAsync(
                        section.CourseId);

                if (course.CreatedByUserId != currentUserId.Value)
                {
                    return Forbid();
                }
            }

            var deleted =
                await _sectionsService.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound(new
                {
                    message =
                        $"No se encontró la sección con ID {id}."
                });
            }

            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new
            {
                message = ex.Message
            });
        }
    }

    private int? GetCurrentUserId()
    {
        var claim =
            User.FindFirst(
                System.Security.Claims.ClaimTypes.NameIdentifier);

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