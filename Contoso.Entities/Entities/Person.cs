using Regira.Entities.Models.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace Contoso.Entities;

public abstract class Person : IEntityWithSerial, IHasTitle, IArchivable
{
    public int Id { get; set; }
    [Required][MaxLength(64)] public string GivenName { get; set; } = null!;
    [Required][MaxLength(64)] public string? LastName { get; set; }
    public bool IsArchived { get; set; }

    public string Title => $"{GivenName} {LastName}".Trim();
}

public class PersonDto
{
    public int Id { get; set; }
    public string GivenName { get; set; } = null!;
    public string? LastName { get; set; }
}

public class PersonInputDto
{
    public int Id { get; set; }
    [Required][MaxLength(64)] public string GivenName { get; set; } = null!;
    [Required][MaxLength(64)] public string? LastName { get; set; }
}