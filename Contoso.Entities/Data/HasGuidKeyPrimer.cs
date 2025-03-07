using Microsoft.EntityFrameworkCore.ChangeTracking;
using Regira.Entities.EFcore.Primers.Abstractions;
using Regira.Entities.Models.Abstractions;

namespace Contoso.Data;

internal class HasGuidKeyPrimer : EntityPrimerBase<IEntity<Guid>>
{
    public override Task PrepareAsync(IEntity<Guid> entity, EntityEntry entry)
    {
        if (entity.Id == Guid.Empty)
        {
            entity.Id = Guid.NewGuid();
        }

        return Task.CompletedTask;
    }
}