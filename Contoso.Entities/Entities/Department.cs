using Regira.Entities.Models.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace Contoso.Entities;

public class Department : IEntity<Guid>, IHasTitle, IArchivable
{
    public Guid Id { get; set; }
    public int? InstructorId { get; set; }
    [Required][MaxLength(64)] public string? Title { get; set; }
    public decimal? Budget { get; set; }
    public DateTime StartDate { get; set; }
    public bool IsArchived { get; set; }

    public Instructor? Administrator { get; set; }
    public ICollection<Course>? Courses { get; set; }
}

public class DepartmentDto
{
    public Guid Id { get; set; }
    public int? InstructorId { get; set; }
    public string? Title { get; set; }
    public decimal? Budget { get; set; }
    public DateTime StartDate { get; set; }
    
    public InstructorDto? Administrator { get; set; }
    public ICollection<CourseDto>? Courses { get; set; }
}

public class DepartmentInputDto
{
    public Guid Id { get; set; }
    public int? InstructorId { get; set; }
    public string? Title { get; set; }
    public decimal? Budget { get; set; }
    public DateTime StartDate { get; set; }
}