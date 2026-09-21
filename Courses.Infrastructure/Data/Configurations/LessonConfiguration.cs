using System;
using System.Collections.Generic;
using System.Text;
using Courses.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Courses.Infrastructure.Data.Configurations;

public class LessonConfiguration : IEntityTypeConfiguration<Lesson>
{
    public void Configure(EntityTypeBuilder<Lesson> builder)
    {
        builder.ToTable("Lessons");

        builder.HasKey(x => x.LessonId);

        builder.Property(x => x.Title)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasMaxLength(1000);

        builder.Property(x => x.ContentType)
            .HasMaxLength(50)
            .IsRequired();
        
        builder.Property(x => x.ContentUrl)
            .HasMaxLength(1000)
            .IsRequired();

        builder.Property(x => x.LessonOrder)
            .IsRequired();

        builder.Property(x => x.IsPreview);

        builder.Property(x => x.CreatedAt)
            .HasDefaultValueSql("SYSDATETIME()");

        // CourseSection → Lessons

        builder.HasOne(x => x.Section)
            .WithMany(x => x.Lessons)
            .HasForeignKey(x => x.SectionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => new 
        { 
            x.SectionId, 
            x.LessonOrder 
        })
            .IsUnique();
    }
}