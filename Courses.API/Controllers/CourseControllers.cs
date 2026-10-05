using System.Security.Claims;
using Courses.Application.DTOs.Courses;
using Courses.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Courses.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CoursesController : ControllerBase
{
    private readonly ICoursesService _coursesService;

    public CoursesController(ICoursesService coursesService)
    {
        _coursesService = coursesService;
    }

    // GET: api/courses
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CourseDto>>> GetAll()
    {
        var courses = await _coursesService.GetAllAsync();

        return Ok(courses);
    }

    // GET: api/courses/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<CourseDto>> GetById(int id)
    {
        try
        {
            var course = await _coursesService.GetByIdAsync(id);

            return Ok(course);
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new
            {
                message = $"No se encontró el curso con ID {id}."
            });
        }
    }

    // POST: api/courses
    [HttpPost]
    [Authorize(Roles = "Admin,Instructor")]
    public async Task<ActionResult<CourseDto>> Create(
        [FromBody] CreateCourseDto createCourseDto)
    {
        var currentUserId = GetCurrentUserId();

        if (currentUserId is null)
        {
            return Unauthorized();
        }

        // El creador SIEMPRE sale del JWT
        createCourseDto.CreatedByUserId = currentUserId.Value;

        var course =
            await _coursesService.CreateAsync(createCourseDto);

        return CreatedAtAction(
            nameof(GetById),
            new { id = course.CourseId },
            course);
    }

    // PUT: api/courses/5
    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin,Instructor")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] UpdateCourseDto updateCourseDto)
    {
        try
        {
            var course = await _coursesService.GetByIdAsync(id);

            // Admin puede modificar cualquier curso
            if (!User.IsInRole("Admin"))
            {
                var currentUserId = GetCurrentUserId();

                if (currentUserId is null)
                {
                    return Unauthorized();
                }

                // Instructor solo puede modificar sus propios cursos
                if (course.CreatedByUserId != currentUserId.Value)
                {
                    return Forbid();
                }
            }

            var updated = await _coursesService.UpdateAsync(
                id,
                updateCourseDto);

            if (!updated)
            {
                return NotFound(new
                {
                    message =
                        $"No se encontró el curso con ID {id}."
                });
            }

            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new
            {
                message =
                    $"No se encontró el curso con ID {id}."
            });
        }
    }

    // DELETE: api/courses/5
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin,Instructor")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var course = await _coursesService.GetByIdAsync(id);

            // Admin puede eliminar cualquier curso
            if (!User.IsInRole("Admin"))
            {
                var currentUserId = GetCurrentUserId();

                if (currentUserId is null)
                {
                    return Unauthorized();
                }

                // Instructor solo puede eliminar sus propios cursos
                if (course.CreatedByUserId != currentUserId.Value)
                {
                    return Forbid();
                }
            }

            var deleted =
                await _coursesService.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound(new
                {
                    message =
                        $"No se encontró el curso con ID {id}."
                });
            }

            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new
            {
                message =
                    $"No se encontró el curso con ID {id}."
            });
        }
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