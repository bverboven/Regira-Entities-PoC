using Regira.Entities.Attachments.Abstractions;
using Regira.Entities.Attachments.Models;
using Regira.Entities.Models.Abstractions;
using Regira.Entities.Web.Attachments.Models;
using System.ComponentModel.DataAnnotations;

namespace Contoso.Entities;

public class Instructor : Person, IHasStartEndDate, IHasAttachments, IHasAttachments<InstructorAttachment>
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public ICollection<Course>? Courses { get; set; }
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
    public string? AttachmentType { get; set; }
}