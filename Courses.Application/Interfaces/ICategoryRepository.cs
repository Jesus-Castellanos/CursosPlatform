using Courses.Domain.Entities;

namespace Courses.Application.Interfaces;

public interface ICategoryRepository
{
    Task<List<Category>> GetAllAsync();

    Task<Category?> GetByIdAsync(int id);

    Task<Category?> GetByNameAsync(string name);

    Task<Category> AddAsync(Category category);

    Task<Category> UpdateAsync(Category category);

    Task DeleteAsync(Category category);
}