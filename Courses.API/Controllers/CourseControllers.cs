using Courses.Application.DTOs.Courses;
using Courses.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Courses.API.Controllers;

[ApiController]
[Route("api/[controller]")]
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
    public async Task<ActionResult<CourseDto>> Create(
        [FromBody] CreateCourseDto createCourseDto)
    {
        var course = await _coursesService.CreateAsync(createCourseDto);

        return CreatedAtAction(
            nameof(GetById),
            new { id = course.CourseId },
            course);
    }


    // PUT: api/courses/5
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] UpdateCourseDto updateCourseDto)
    {
        var updated = await _coursesService.UpdateAsync(
            id,
            updateCourseDto);

        if (!updated)
        {
            return NotFound(new
            {
                message = $"No se encontró el curso con ID {id}."
            });
        }

        return NoContent();
    }


    // DELETE: api/courses/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _coursesService.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound(new
            {
                message = $"No se encontró el curso con ID {id}."
            });
        }

        return NoContent();
    }
}