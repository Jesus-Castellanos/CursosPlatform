using System;
using System.Collections.Generic;
using System.Text;

namespace Courses.Application.DTOs.Categories;

public class CreateCategoryDto
{
    public string Name { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
}