using System.ComponentModel.DataAnnotations;
using Regira.Entities.Models.Abstractions;

namespace Contoso.Entities;

public class OfficeAssignment : IEntityWithSerial, IHasStartEndDate
{
    public int Id { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    [Required][MaxLength(64)] public string Location { get; set; } = null!;
}