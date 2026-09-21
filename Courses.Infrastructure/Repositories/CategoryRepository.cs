using Courses.Application.Interfaces;
using Courses.Domain.Entities;
using Courses.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace Courses.Infrastructure.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly CoursesDbContext _context;

    public CategoryRepository(CoursesDbContext context)
    {
        _context = context;
    }

    public async Task<List<Category>> GetAllAsync()
    {
        return await _context.Categories
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Category?> GetByIdAsync(int id)
    {
        return await _context.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.CategoryId == id);
    }

    public async Task<Category?> GetByNameAsync(string name)
    {
        return await _context.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(x =>
                EF.Functions.Collate(
                    x.Name,
                    "Latin1_General_100_CI_AS") == name);
    }

    public async Task<Category> AddAsync(Category category)
    {
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        return category;
    }

    public async Task<Category> UpdateAsync(Category category)
    {
        _context.Categories.Update(category);
        await _context.SaveChangesAsync();

        return category;
    }

    public async Task DeleteAsync(Category category)
    {
        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
    }
}