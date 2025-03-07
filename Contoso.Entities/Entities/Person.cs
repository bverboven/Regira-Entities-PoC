using System.ComponentModel.DataAnnotations;
using Regira.Entities.Models.Abstractions;

namespace Contoso.Entities;

public class Person : IEntityWithSerial, IHasTitle, IArchivable
{
    public int Id { get; set; }
    [Required][MaxLength(64)] public string GivenName { get; set; } = null!;
    [Required][MaxLength(64)] public string? LastName { get; set; }
    public bool IsArchived { get; set; }

    public string Title => $"{GivenName} {LastName}".Trim();
}