using Courses.Application.DTOs.Lessons;
using Courses.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Courses.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LessonsController : ControllerBase
{
    private readonly ILessonsService _lessonsService;
    private readonly ICourseSectionsService _sectionsService;
    private readonly ICoursesService _coursesService;

    public LessonsController(
        ILessonsService lessonsService,
        ICourseSectionsService sectionsService,
        ICoursesService coursesService)
    {
        _lessonsService = lessonsService;
        _sectionsService = sectionsService;
        _coursesService = coursesService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<LessonDto>>> GetAll()
    {
        var lessons =
            await _lessonsService.GetAllAsync();

        // Admin e Instructor pueden ver todas las lecciones.
        if (User.IsInRole("Admin") ||
            User.IsInRole("Instructor"))
        {
            return Ok(lessons);
        }

        // Obtener cursos visibles para Student.
        var courses =
            await _coursesService.GetAllAsync();

        var visibleCourseIds = courses
            .Where(x => x.IsPublished && x.IsActive)
            .Select(x => x.CourseId)
            .ToHashSet();

        // Obtener secciones pertenecientes a cursos visibles.
        var sections =
            await _sectionsService.GetAllAsync();

        var visibleSectionIds = sections
            .Where(x => visibleCourseIds.Contains(x.CourseId))
            .Select(x => x.SectionId)
            .ToHashSet();

        var visibleLessons = lessons
            .Where(x => visibleSectionIds.Contains(x.SectionId));

        return Ok(visibleLessons);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<LessonDto>> GetById(int id)
    {
        var lesson =
            await _lessonsService.GetByIdAsync(id);

        if (lesson is null)
        {
            return NotFound(new
            {
                message =
                    $"No se encontró la lección con ID {id}."
            });
        }

        // Admin e Instructor pueden consultar cualquier lección.
        if (User.IsInRole("Admin") ||
            User.IsInRole("Instructor"))
        {
            return Ok(lesson);
        }

        // Student solamente puede consultar
        // lecciones de cursos publicados y activos.
        var section =
            await _sectionsService.GetByIdAsync(
                lesson.SectionId);

        if (section is null)
        {
            return NotFound(new
            {
                message =
                    $"No se encontró la lección con ID {id}."
            });
        }

        var course =
            await _coursesService.GetByIdAsync(
                section.CourseId);

        if (!course.IsPublished ||
            !course.IsActive)
        {
            return NotFound(new
            {
                message =
                    $"No se encontró la lección con ID {id}."
            });
        }

        return Ok(lesson);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Instructor")]
    public async Task<ActionResult<LessonDto>> Create(
        [FromBody] CreateLessonDto dto)
    {
        try
        {
            // Instructor solo puede crear lecciones
            // dentro de sus propios cursos.
            if (User.IsInRole("Instructor"))
            {
                var currentUserId = GetCurrentUserId();

                if (currentUserId is null)
                {
                    return Unauthorized();
                }

                var section =
                    await _sectionsService.GetByIdAsync(
                        dto.SectionId);

                if (section is null)
                {
                    return NotFound(new
                    {
                        message =
                            $"No se encontró la sección con ID {dto.SectionId}."
                    });
                }

                var course =
                    await _coursesService.GetByIdAsync(
                        section.CourseId);

                if (course.CreatedByUserId != currentUserId.Value)
                {
                    return Forbid();
                }
            }

            var lesson =
                await _lessonsService.AllAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = lesson.LessonId },
                lesson);
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
        [FromBody] UpdateLessonDto dto)
    {
        try
        {
            var existing =
                await _lessonsService.GetByIdAsync(id);

            if (existing is null)
            {
                return NotFound(new
                {
                    message =
                        $"No se encontró la lección con ID {id}."
                });
            }

            // Instructor solo puede modificar
            // lecciones de sus propios cursos.
            if (User.IsInRole("Instructor"))
            {
                var currentUserId = GetCurrentUserId();

                if (currentUserId is null)
                {
                    return Unauthorized();
                }

                var section =
                    await _sectionsService.GetByIdAsync(
                        existing.SectionId);

                if (section is null)
                {
                    return NotFound(new
                    {
                        message =
                            $"No se encontró la sección con ID {existing.SectionId}."
                    });
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
                await _lessonsService.UpdateAsync(id, dto);

            if (!updated)
            {
                return NotFound(new
                {
                    message =
                        $"No se encontró la lección con ID {id}."
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
            var existing =
                await _lessonsService.GetByIdAsync(id);

            if (existing is null)
            {
                return NotFound(new
                {
                    message =
                        $"No se encontró la lección con ID {id}."
                });
            }

            // Instructor solo puede eliminar
            // lecciones de sus propios cursos.
            if (User.IsInRole("Instructor"))
            {
                var currentUserId = GetCurrentUserId();

                if (currentUserId is null)
                {
                    return Unauthorized();
                }

                var section =
                    await _sectionsService.GetByIdAsync(
                        existing.SectionId);

                if (section is null)
                {
                    return NotFound(new
                    {
                        message =
                            $"No se encontró la sección con ID {existing.SectionId}."
                    });
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
                await _lessonsService.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound(new
                {
                    message =
                        $"No se encontró la lección con ID {id}."
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