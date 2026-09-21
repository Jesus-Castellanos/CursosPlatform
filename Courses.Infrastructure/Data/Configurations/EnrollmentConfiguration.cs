using System;
using System.Collections.Generic;
using System.Text;
using Courses.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Courses.Infrastructure.Data.Configurations;

public class EnrollmentConfiguration : IEntityTypeConfiguration<Enrollment>
{
    public void Configure(EntityTypeBuilder<Enrollment> builder)
    {
        builder.ToTable("Enrollments");
        
        builder.HasKey(x => x.EnrollmentId);

        builder.Property(x => x.EnrollmentDate)
            .HasDefaultValueSql("SYSDATETIME()");

        builder.Property(x => x.CompletionPercentage)
            .HasPrecision(5, 2)
            .HasDefaultValue(0);

        builder.HasOne(x => x.User)
            .WithMany(u => u.Enrollments)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Course)
            .WithMany(c => c.Enrollments)
            .HasForeignKey(x => x.CourseId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => new 
        { 
            x.UserId, 
            x.CourseId 
        })
            .IsUnique();
    }
}
