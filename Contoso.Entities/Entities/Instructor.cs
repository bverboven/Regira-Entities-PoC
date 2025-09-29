using Regira.Entities.Attachments.Abstractions;
using Regira.Entities.Attachments.Models;
using Regira.Entities.Mapping.Models;
using System.ComponentModel.DataAnnotations;

namespace Contoso.Entities;

public class Instructor : Person, IHasAttachments, IHasAttachments<InstructorAttachment>
{
    public ICollection<CourseInstructor>? Courses { get; set; }
    public ICollection<OfficeAssignment>? OfficeAssignments { get; set; }

    public bool? HasAttachment { get; set; }
    public ICollection<InstructorAttachment>? Attachments { get; set; }
    ICollection<IEntityAttachment>? IHasAttachments.Attachments
    {
        get => Attachments?.Cast<IEntityAttachment>().ToList();
        set => Attachments = value?.Cast<InstructorAttachment>().ToList();
    }
}


public class InstructorAttachment : EntityAttachment
{
    [MaxLength(32)]
    public string? AttachmentType { get; set; }
}

public class InstructorAttachmentDto : EntityAttachmentDto
{
    public string? AttachmentType { get; set; }
}
public class InstructorAttachmentInputDto : EntityAttachmentInputDto
{
    [MaxLength(64)]
    public string? AttachmentType { get; set; }
}

public class InstructorDto : PersonDto
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public ICollection<CourseInstructorDto>? Courses { get; set; }
    public ICollection<OfficeAssignmentDto>? OfficeAssignments { get; set; }
    public ICollection<InstructorAttachmentDto>? Attachments { get; set; }
}
public class InstructorInputDto : PersonInputDto
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public ICollection<CourseInstructorInputDto>? Courses { get; set; }
    public ICollection<InstructorAttachmentInputDto>? Attachments { get; set; }
}