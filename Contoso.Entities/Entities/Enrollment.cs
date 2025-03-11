using Contoso.Constants;
using Regira.Entities.Models.Abstractions;

namespace Contoso.Entities;

public class Enrollment : IEntityWithSerial, IHasStartDate
{
    public int Id { get; set; }
    public int CourseId { get; set; }
    public int StudentId { get; set; }
    public Grade? Grade { get; set; }
    public DateTime? StartDate { get; set; }

    public Course? Course { get; set; }
    public Student? Student { get; set; }
}

public class EnrollmentDto
{
    public int Id { get; set; }
    public int CourseId { get; set; }
    public int StudentId { get; set; }
    public Grade? Grade { get; set; }
    public DateTime? StartDate { get; set; }

    public CourseDto? Course { get; set; }
    public StudentDto? Student { get; set; }
}

public class EnrollmentInputDto
{
    public int Id { get; set; }
    public int CourseId { get; set; }
    public int StudentId { get; set; }
    public Grade? Grade { get; set; }
    public DateTime? StartDate { get; set; }
}