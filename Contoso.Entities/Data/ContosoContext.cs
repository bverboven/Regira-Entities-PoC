using Contoso.Entities;
using Microsoft.EntityFrameworkCore;
using Regira.Entities.Attachments.Models;

namespace Contoso.Data;

public class ContosoContext(DbContextOptions<ContosoContext> options) : DbContext(options)
{
    public DbSet<Attachment> Attachments { get; set; }
    public DbSet<Person> Persons { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Instructor> Instructors { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }
    public DbSet<Course> Courses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Course>(e =>
        {
            e.HasMany(c => c.Attachments).WithOne().HasForeignKey(a => a.ObjectId);
        });

        modelBuilder.Entity<Instructor>(e =>
        {
            e.HasMany(c => c.Attachments).WithOne().HasForeignKey(a => a.ObjectId);
            e.HasMany(c => c.Attachments).WithOne().HasForeignKey(a => a.ObjectId);
        });
    }
}
