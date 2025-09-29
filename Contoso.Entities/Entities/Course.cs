using AutoMapper;
using Regira.Entities.Attachments.Abstractions;
using Regira.Entities.Attachments.Models;
using Regira.Entities.Mapping.Models;
using Regira.Entities.Models;
using Regira.Entities.Models.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace Contoso.Entities;
public class Course : IEntityWithSerial, IHasTitle, IArchivable, IHasAttachments, IHasAttachments<CourseAttachment>
{
    public int Id { get; set; }
    public Guid DepartmentId { get; set; }
    [Required][MaxLength(64)] public string? Title { get; set; }
    [Range(0, 5)] public int? Credits { get; set; }
    public bool IsArchived { get; set; }


    public Department? Department { get; set; }
    public ICollection<Enrollment>? Enrollments { get; set; }
    public ICollection<CourseInstructor>? Instructors { get; set; }

    public bool? HasAttachment { get; set; }
    public ICollection<CourseAttachment>? Attachments { get; set; }
    ICollection<IEntityAttachment>? IHasAttachments.Attachments
    {
        get => Attachments?.Cast<IEntityAttachment>().ToList();
        set => Attachments = value?.Cast<CourseAttachment>().ToList();
    }
}
public class CourseInstructor : IEntityWithSerial, IHasStartEndDate
{
    public int Id { get; set; }
    public int InstructorId { get; set; }
    public int CourseId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public Course? Course { get; set; }
    public Instructor? Instructor { get; set; }
}

public class CourseAttachment : EntityAttachment;

public class CourseSearchObject : SearchObject
{
    public ICollection<Guid>? DepartmentId { get; set; }
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
    TitleDesc,
    Department,
    EnrollmentDate,
    EnrollmentDateDesc,
}

public enum CourseIncludes
{
    None = 0,
    Department = 1 << 0,
    Instructors = 1 << 1,
    Enrollments = 1 << 2,
    Students = 1 << 3,
    Attachments = 1 << 4,
    All = Department | Enrollments | Instructors | Attachments
}

public class CourseDto
{
    public int Id { get; set; }
    public Guid DepartmentId { get; set; }
    public string? Title { get; set; }
    public int? Credits { get; set; }


    public DepartmentDto? Department { get; set; }
    public ICollection<EnrollmentDto>? Enrollments { get; set; }
    public ICollection<CourseInstructorDto>? Instructors { get; set; }
    public ICollection<EntityAttachmentDto>? Attachments { get; set; }
}

public class CourseInstructorDto
{
    public int Id { get; set; }
    public int InstructorId { get; set; }
    public int CourseId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public CourseDto? Course { get; set; }
    public InstructorDto? Instructor { get; set; }
}
public class CourseInstructorInputDto
{
    public int Id { get; set; }
    public int InstructorId { get; set; }
    public int CourseId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}

public class CourseInputDto
{
    public int Id { get; set; }
    public Guid DepartmentId { get; set; }
    [Required][MaxLength(64)] public string? Title { get; set; }
    [Range(0, 5)] public int? Credits { get; set; }
    public ICollection<EnrollmentInputDto>? Enrollments { get; set; }
    public ICollection<CourseInstructorInputDto>? Instructors { get; set; }
}

public class CourseProfile : Profile
{
    public CourseProfile()
    {
        CreateMap<Course, CourseDto>();
        CreateMap<CourseInputDto, Course>();
        CreateMap<CourseInstructor, CourseInstructorDto>();
        CreateMap<CourseInstructorInputDto, CourseInstructor>();
        CreateMap<Enrollment, EnrollmentDto>();
        CreateMap<EnrollmentInputDto, Enrollment>();
    }
}