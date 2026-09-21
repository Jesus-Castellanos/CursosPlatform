using Courses.Application.DTOs.Categories;
using Courses.Application.Interfaces;
using Courses.Domain.Entities;

namespace Courses.Application.Services;

public class CategoriesService : ICategoriesService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoriesService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<IEnumerable<CategoryDto>> GetAllAsync()
    {
        var categories = await _categoryRepository.GetAllAsync();

        return categories.Select(MapToDto);
    }

    public async Task<CategoryDto?> GetByIdAsync(int id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);

        return category is null
            ? null
            : MapToDto(category);
    }

    public async Task<CategoryDto> CreateAsync(
        CreateCategoryDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            throw new ArgumentException(
                "El nombre de la categoría es obligatorio.");
        }

        string name = dto.Name.Trim();

        var existing =
            await _categoryRepository.GetByNameAsync(name);

        if (existing is not null)
        {
            throw new InvalidOperationException(
                "Ya existe una categoría con ese nombre.");
        }

        var category = new Category
        {
            Name = name,
            Description = string.IsNullOrWhiteSpace(dto.Description)
                ? null
                : dto.Description.Trim(),
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        var created =
            await _categoryRepository.AddAsync(category);

        return MapToDto(created);
    }

    public async Task<bool> UpdateAsync(
        int id,
        UpdateCategoryDto dto)
    {
        var category =
            await _categoryRepository.GetByIdAsync(id);

        if (category is null)
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            throw new ArgumentException(
                "El nombre de la categoría es obligatorio.");
        }

        string name = dto.Name.Trim();

        var existing =
            await _categoryRepository.GetByNameAsync(name);

        if (existing is not null &&
            existing.CategoryId != id)
        {
            throw new InvalidOperationException(
                "Ya existe otra categoría con ese nombre.");
        }

        category.Name = name;
        category.Description =
            string.IsNullOrWhiteSpace(dto.Description)
                ? null
                : dto.Description.Trim();

        category.IsActive = dto.IsActive;

        await _categoryRepository.UpdateAsync(category);

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var category =
            await _categoryRepository.GetByIdAsync(id);

        if (category is null)
        {
            return false;
        }

        await _categoryRepository.DeleteAsync(category);

        return true;
    }

    private static CategoryDto MapToDto(Category category)
    {
        return new CategoryDto
        {
            CategoryId = category.CategoryId,
            Name = category.Name,
            Description = category.Description,
            CreatedAt = category.CreatedAt,
            IsActive = category.IsActive
        };
    }
}