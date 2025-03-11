using Regira.Entities.Models.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace Contoso.Entities;

public class OfficeAssignment : IEntityWithSerial, IHasStartEndDate
{
    public int Id { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    [Required][MaxLength(64)] public string Location { get; set; } = null!;
}

public class OfficeAssignmentDto
{
    public int Id { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string Location { get; set; } = null!;
}

public class OfficeAssignmentInputDto
{
    public int Id { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    [Required][MaxLength(64)] public string Location { get; set; } = null!;
}