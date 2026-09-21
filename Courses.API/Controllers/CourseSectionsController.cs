using Courses.Application.DTOs.CourseSections;
using Courses.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Courses.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CourseSectionsController : ControllerBase
{
    private readonly ICourseSectionsService _sectionsService;

    public CourseSectionsController(
        ICourseSectionsService sectionsService)
    {
        _sectionsService = sectionsService;
    }

    [HttpGet]
    public async Task<
        ActionResult<IEnumerable<CourseSectionDto>>> GetAll()
    {
        var sections =
            await _sectionsService.GetAllAsync();

        return Ok(sections);
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

        return Ok(section);
    }

    [HttpPost]
    public async Task<ActionResult<CourseSectionDto>> Create(
        [FromBody] CreateCourseSectionDto dto)
    {
        try
        {
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
    public async Task<IActionResult> Update(
        int id,
        [FromBody] UpdateCourseSectionDto dto)
    {
        try
        {
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
}