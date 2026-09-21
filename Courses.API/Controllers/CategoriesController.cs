using Courses.Application.DTOs.Categories;
using Courses.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Courses.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoriesService _categoriesService;

    public CategoriesController(
        ICategoriesService categoriesService)
    {
        _categoriesService = categoriesService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAll()
    {
        var categories =
            await _categoriesService.GetAllAsync();

        return Ok(categories);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CategoryDto>> GetById(int id)
    {
        var category =
            await _categoriesService.GetByIdAsync(id);

        if (category is null)
        {
            return NotFound(new
            {
                message =
                    $"No se encontró la categoría con ID {id}."
            });
        }

        return Ok(category);
    }

    [HttpPost]
    public async Task<ActionResult<CategoryDto>> Create(
        [FromBody] CreateCategoryDto dto)
    {
        try
        {
            var category =
                await _categoriesService.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = category.CategoryId },
                category);
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
        [FromBody] UpdateCategoryDto dto)
    {
        try
        {
            var updated =
                await _categoriesService.UpdateAsync(id, dto);

            if (!updated)
            {
                return NotFound(new
                {
                    message =
                        $"No se encontró la categoría con ID {id}."
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
            var deleted =
                await _categoriesService.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound(new
                {
                    message =
                        $"No se encontró la categoría con ID {id}."
                });
            }

            return NoContent();
        }
        catch (Exception)
        {
            return Conflict(new
            {
                message =
                    "No se puede eliminar la categoría porque puede estar siendo utilizada por cursos."
            });
        }
    }
}