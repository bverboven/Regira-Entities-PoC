using Regira.Entities.Attachments.Abstractions;
using Regira.Entities.Attachments.Models;
using Regira.Entities.Models;
using Regira.Entities.Models.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace Contoso.Entities;
public class Course : IEntityWithSerial, IHasTitle, IArchivable, IHasAttachments, IHasAttachments<CourseAttachment>
{
    public int Id { get; set; }
    public int DepartmentId { get; set; }
    [Required][MaxLength(64)] public string? Title { get; set; }
    [Range(0, 5)] public int? Credits { get; set; }
    public bool IsArchived { get; set; }


    public Department? Department { get; set; }
    public ICollection<Enrollment>? Enrollments { get; set; }
    public ICollection<Instructor>? Instructors { get; set; }

    public bool? HasAttachment { get; set; }
    public ICollection<CourseAttachment>? Attachments { get; set; }
    ICollection<IEntityAttachment>? IHasAttachments.Attachments
    {
        get => Attachments?.Cast<IEntityAttachment>().ToList();
        set => Attachments = value?.Cast<CourseAttachment>().ToList();
    }
}

public class CourseAttachment : EntityAttachment;

public class CourseSearchObject : SearchObject
{
    public ICollection<int>? DepartmentId { get; set; }
    public ICollection<int>? InstructorId { get; set; }
    public int? MinCredits { get; set; }
    public int? MaxCredits { get; set; }
    public DateTime? MinEnrollmentDate { get; set; }
    public DateTime? MaxEnrollmentDate { get; set; }
}

public enum CourseSortBy
{
    Id,
    IdDesc,
    Title,
    EnrollmentDate,
    EnrollmentDateDesc,
}

public enum CourseIncludes
{
    None = 0,
    Instructors = 1 << 0,
    Enrollments = 1 << 1,
    Students = 1 << 2,
    Attachments = 1 << 3,
    All = Enrollments | Instructors | Attachments
}